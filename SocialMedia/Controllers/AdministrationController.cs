using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using SocialMedia.Models;

namespace SocialMedia.Controllers{
   [Authorize(Roles = "Administrador")] // [Authorize(Roles = "Administrator, Moderador, ...")] [Authorize(Roles = "Administrator] [Authorize(Roles = "Moderador")] [Authorize(Roles = "Moderador")] [Authorize(Roles = "Moderador")] [Authorize(Roles = "Mod
    public class AdministrationController : Controller{
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public AdministrationController(RoleManager<ApplicationRole> roleManager, UserManager<ApplicationUser> userManager){
            _roleManager = roleManager;
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult CreateRole(){
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateRole(CreateRoleViewModel roleModel){
            if(ModelState.IsValid){
                // Verificar si el rol ya existe en la base de datos
                bool roleExists = await _roleManager.RoleExistsAsync(roleModel?.RoleName ?? string.Empty);
                if(roleExists){
                    ModelState.AddModelError(string.Empty, "El rol ya existe.");
                }else{
                    // Crear el nuevo rol y guardarlo 
                    ApplicationRole ApplicationRole = new ApplicationRole{
                        Name = roleModel?.RoleName ?? string.Empty,
                        Description = roleModel?.Description ?? string.Empty
                    };
                    IdentityResult result = await _roleManager.CreateAsync(ApplicationRole);
                    if(result.Succeeded){
                        return RedirectToAction("ListRoles", "Administration");
                    }
                    foreach(IdentityError error in result.Errors){
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            return View(roleModel);
        }

        [HttpGet]
        public async Task<IActionResult> ListRoles() {
            // Obtener todos los roles de la base de datos y mostrarlos en la vista 
            List<ApplicationRole> roles = await _roleManager.Roles.ToListAsync();
            return View(roles);
        }

        [HttpGet]
        public async Task<IActionResult> EditRole(string id) {
            // Obtener el rol de la base de datos y mostrarlo en la vista  para su edición
            ApplicationRole? role = await _roleManager.FindByIdAsync(id);
            if(role == null) {
                return View("Error");
            }
            // Crear un modelo de vista para mostrar el nombre del rol y otras propiedades que deseas editar si es necesario  (Ej: descripción, permisos)
            var model = new EditRoleViewModel { 
                Id = role.Id, 
                RoleName = role.Name ?? string.Empty,
                Description = role.Description ?? string.Empty,
                Users = new List<string>()
                // Puedes agregar otras propiedades aquí si es necesario
            };
            foreach(var user in await _userManager.Users.ToListAsync()) {
                // Agregar los usuarios que están en este rol a la lista de usuarios
                if(await _userManager.IsInRoleAsync(user, role.Name ?? string.Empty)) {
                    model.Users.Add(user?.UserName ?? string.Empty);
                }
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditRole(EditRoleViewModel model) {
            if(ModelState.IsValid) {
                // Obtener el rol de la base de datos
                var role = await _roleManager.FindByIdAsync(model.Id);
                if(role == null) { // El rol no fue encontrado
                    ViewBag.ErrorMessage = $"El rol con ID {model.Id} no fue encontrado.";
                    return View(model);
                }else {
                    // Actualizar el nombre del rol y guardar los cambios
                    role.Name = model.RoleName;
                    role.Description = model.Description;
                    // Validar los cambios y guardarlos en la base de datos
                    var result = await _roleManager.UpdateAsync(role);
                    if(result.Succeeded) {
                        return RedirectToAction("ListRoles", "Administration");
                    }
                    // Mostrar los errores de validación en caso de que haya ocurrido un problema al actualizar el rol  
                    foreach(var error in result.Errors) {
                        ModelState.AddModelError("", error.Description);
                    }
                    return View(model);
                }
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteRole(string id) {
            // Obtener el rol de la base de datos y eliminarlo de la base de datos
            var role = await _roleManager.FindByIdAsync(id);
            if(role == null) { // El rol no fue encontrado
                ViewBag.ErrorMessage = $"El rol con ID {id} no fue encontrado.";
                return View("ListRoles", await _roleManager.Roles.ToListAsync());
            }
            // Validar los cambios y eliminar el rol en la base de datos
            var result = await _roleManager.DeleteAsync(role);
            if(result.Succeeded) {
                return RedirectToAction("ListRoles", "Administration");
            }
            // Mostrar los errores de validación en caso de que haya ocurrido un problema
            foreach(var error in result.Errors) {
                ModelState.AddModelError("", error.Description);
            }
            // Regresar a la lista de roles después de eliminar el rol
            return View("ListRoles", await _roleManager.Roles.ToListAsync());
        }

        [HttpGet]
        public async Task<IActionResult> EditUsersInRole(string id)
        {
            // Obtener el rol de la base de datos y mostrar los usuarios que están en ese rol en la vista para su edición
            ViewBag.id = id;
            var role = await _roleManager.FindByIdAsync(id);

            if (role == null) {
                ViewBag.ErrorMessage = $"Role with Id = {id} cannot be found";
                return View("NotFound");
            }
            // Crear un modelo de vista para mostrar los usuarios y seleccionar los que están en
            ViewBag.RollName = role.Name;
            var model = new List<UserRoleViewModel>();
            // Agregar los usuarios que están en este rol a la lista de usuarios
            foreach (var user in _userManager.Users.ToList()) {
                if (user == null) { continue; }

                var userRoleViewModel = new UserRoleViewModel {
                    UserId = user.Id,
                    UserName = user.UserName ?? string.Empty,
                };

                // Manejo de nulabilidad de role y role.Name
                if (role != null && !string.IsNullOrEmpty(role.Name) && await _userManager.IsInRoleAsync(user, role.Name)){
                    userRoleViewModel.IsSelected = true;
                } else {
                    userRoleViewModel.IsSelected = false;
                }

                model.Add(userRoleViewModel);
            }


            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditUsersInRole(List<UserRoleViewModel> model, string id)
        {
            // Primero verifique si el rol existe o no
            var role = await _roleManager.FindByIdAsync(id);

            if (role == null) {
                ViewBag.ErrorMessage = $"Rol con id = {id} no fue encontrado.";
                return View("NotFound");
            }
            // Eliminar todos los usuarios del rol actual
            for (int i = 0; i < model.Count; i++) {
                var user = await _userManager.FindByIdAsync(model[i].UserId);                
                // Verificar si user es null antes de proceder
                if (user == null) continue;  

                IdentityResult? result = null;
                string roleName = role?.Name ?? string.Empty; // Manejar nulabilidad de role y role.Name

                // Verificar si roleName no es vacío y luego proceder
                if (!string.IsNullOrEmpty(roleName))
                {
                    if (model[i].IsSelected && !(await _userManager.IsInRoleAsync(user, roleName))) {
                        // Si IsSelected es verdadero y el usuario no está en este rol, agregar al usuario
                        result = await _userManager.AddToRoleAsync(user, roleName);
                    } else if (!model[i].IsSelected && await _userManager.IsInRoleAsync(user, roleName)) {
                        // Si IsSelected es falso y el usuario está en este rol, eliminar al usuario
                        result = await _userManager.RemoveFromRoleAsync(user, roleName);
                    }
                }

                // Si se realizó una operación, verificar si result no es null y si es exitosa
                if (result != null && result.Succeeded) {
                    if (i < (model.Count - 1))  continue; 
                    else return RedirectToAction("EditRole", new { id = id }); 
                }
            }
            return RedirectToAction("EditRole", new { id = id });
        }

    }
}