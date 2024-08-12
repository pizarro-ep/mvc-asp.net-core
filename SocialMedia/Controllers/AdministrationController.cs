using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using SocialMedia.Models;

namespace SocialMedia.Controllers{
   [Authorize(Roles = "Administrador")] // [Authorize(Roles = "Administrator, Moderador, ...")] [Authorize(Roles = "Administrator] [Authorize(Roles = "Moderador")] [Authorize(Roles = "Moderador")] [Authorize(Roles = "Moderador")] [Authorize(Roles = "Mod
    public class AdministrationController : Controller{
        private readonly IAuthorizationService _authorizationService;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public AdministrationController(IAuthorizationService authorizationService, RoleManager<ApplicationRole> roleManager, UserManager<ApplicationUser> userManager){
            _authorizationService = authorizationService;
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

            // Verificar si el usuario tiene permisos para editar y eliminar roles
            var canEdit = await _authorizationService.AuthorizeAsync(User, "EditRolePolicy");
            var canDelete = await _authorizationService.AuthorizeAsync(User, "DeleteRolePolicy");
            // Crear un modelo de vista para mostrar los roles y otros detalles que deseas mostrar
            var viewModel = new RoleViewModel{
                Roles = roles,
                CanEdit = canEdit.Succeeded,
                CanDelete = canDelete.Succeeded
            };

            return View(viewModel);
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
                //Users = new List<string>()
                // Puedes agregar otras propiedades aquí si es necesario
            };
            // Agregar los usuarios y los claims al modelo si es necesario
            model.Users = new List<string>();
            model.Claims = new List<string>();

            // Obtener los claims del rol y agregarlos al modelo
            var roleClaims = await _roleManager.GetClaimsAsync(role);
            model.Claims = roleClaims.Select(c => c.Value).ToList();

            foreach(var user in await _userManager.Users.ToListAsync()) {
                // Agregar los usuarios que están en este rol a la lista de usuarios
                if(await _userManager.IsInRoleAsync(user, role.Name ?? string.Empty)) {
                    model.Users.Add(user?.UserName ?? string.Empty);
                }
            }

            return View(model);
        }

        [HttpPost]
        [Authorize(Policy = "EditRolePolicy")]
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
        [Authorize(Policy = "DeleteRolePolicy")]
        public async Task<IActionResult> DeleteRole(string id) {
            // Obtener el rol de la base de datos y eliminarlo de la base de datos
            var role = await _roleManager.FindByIdAsync(id);
            if(role == null) { // El rol no fue encontrado
                ViewBag.ErrorMessage = $"El rol con ID {id} no fue encontrado.";
                return View("NotFound");
            }
            try {
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
            } catch (DbUpdateException ex) {
                // Log de la excepción a un archivo
                ViewBag.Error = ex.Message;
                // Pasa la información del título y del mensaje de error que deseas mostrar al usuario en la v
                // La vista Error recupera esta información de la vista y la muestra al usuario.
                ViewBag.ErrorTitle = $"{role.Name} Role is in Use";
                ViewBag.ErrorMessage = $"{role.Name} Role cannot be deleted as there are users in this role. If you want to delete this role, please remove the users from the role and then try to delete";
                return View("~/Views/Error/Error.cshtml");
            }
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

        [HttpGet]
        public IActionResult ListUsers(){
            var users = _userManager.Users;
            return View(users);
        }

        [HttpGet]
        [Authorize(Policy = "EditRolePolicy")]
        public async Task<IActionResult> EditUser(string id){
            // Obtener el usuario de la base de datos por su ID
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) { // El usuario no fue encontrado
                ViewBag.ErrorMessage = $"El usuario con ID {id} no fue encontrado.";
                return View("NotFound");
            }
            // Crear un modelo de vista para mostrar los detalles del usuario y otras propiedades
            var userClaims = await _userManager.GetClaimsAsync(user);
            // Crear una lista de roles y seleccionar los que el usuario tiene
            var userRoles = await _userManager.GetRolesAsync(user);
            // Crear un modelo de vista para mostrar los detalles del usuario y los roles
            var model = new EditUserViewModel {
                Id = user.Id,
                UserName = user.UserName ?? string.Empty,
                Email = user.Email ?? string.Empty,
                Name = user.Name,
                LastName = user.LastName,
                Claims = userClaims.Select(c => c.Value).ToList(),
                Roles = userRoles
            };
            return View(model);
        }

        [HttpPost]
        [Authorize(Policy = "EditRolePolicy")]
        public async Task<IActionResult> EditUser(EditUserViewModel model) {
            // Obtener el usuario de la base de datos por su ID
            var user = await _userManager.FindByIdAsync(model.Id);
            if (user == null) { // El usuario no fue encontrado
                ViewBag.ErrorMessage = $"El usuario con ID {model.Id} no fue encontrado.";
                return View("NotFound");
            }else{
                // Actualizar los detalles del usuario
                user.Email = model.Email;
                user.UserName = model.UserName;
                user.Name = model.Name;
                user.LastName = model.LastName;
                // Validar los cambios y guardarlos en la base de datos
                var result = await _userManager.UpdateAsync(user);
                if(result.Succeeded) {
                    return RedirectToAction("ListUsers", "Administration");
                }else{
                    foreach(var error in result.Errors) {
                        ModelState.AddModelError("", error.Description);
                    }
                }
                return View(model);
            }
        }

        [HttpPost]
        [Authorize(Policy = "DeleteRolePolicy")]
        public async Task<IActionResult> DeleteUser(string id) {
            // Obtener el usuario de la base de datos por su ID
            var user = await _userManager.FindByIdAsync(id);
            if(user == null) { // El usuario no fue encontrado
                ViewBag.ErrorMessage = $"El usuario con ID {id} no fue encontrado.";
                return View("NotFound");
            }else {
                // Validar los cambios y eliminar el usuario de la base de datos
                var result = await _userManager.DeleteAsync(user);
                if(result.Succeeded) {
                    return RedirectToAction("ListUsers", "Administration");
                }else {
                    foreach(var error in result.Errors) {
                        ModelState.AddModelError("", error.Description);
                    }
                }
                return View("ListUsers");
            }
        }

        [HttpGet]
        public async Task<IActionResult> ManageUserRoles(string id) {
            // Obtener el usuario de la base de datos por su ID
            var user = await _userManager.FindByIdAsync(id);
            if(user == null) { // El usuario no fue encontrado
                ViewBag.ErrorMessage = $"El usuario con ID {id} no fue encontrado.";
                return View("NotFound");
            }
            ViewBag.UserId = id;
            ViewBag.UserName = user.UserName;
            // Crear un modelo de vista para mostrar los roles
            var model = new List<UserRolesViewModel>();
            // Agregar los roles a la lista de roles
            foreach(var role in await _roleManager.Roles.ToListAsync()){ 
                var userRoleViewModel = new UserRolesViewModel {
                    RoleId = role.Id,
                    RoleName = role.Name ?? string.Empty,
                    Description = role.Description,
                };
                // Verificar si el usuario está en este rol y seleccionarlo                
                if(!string.IsNullOrEmpty(role.Name) && await _userManager.IsInRoleAsync(user, role.Name)){
                    userRoleViewModel.IsSelected = true;
                } else {
                    userRoleViewModel.IsSelected = false;
                }
                // Agregar el modelo al modelo de vista
                model.Add(userRoleViewModel);
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ManageUserRoles(List<UserRolesViewModel> model, string id) {
            // Obtener el usuario de la base de datos por su ID
            var user = await _userManager.FindByIdAsync(id);
            if(user == null) { // El usuario no fue encontrado
                ViewBag.ErrorMessage = $"El usuario con ID {id} no fue encontrado.";
                return View("NotFound");
            }
            // Eliminar todos los roles del usuario actual
            var roles = await _userManager.GetRolesAsync(user);
            var result = await _userManager.RemoveFromRolesAsync(user, roles);
            if(!result.Succeeded) {
                ModelState.AddModelError("", "Hubo un problema al eliminar los roles del usuario.");
                return View(model);
            }
            // Agregar los roles seleccionados al usuario
            List<string> RolesToBeAssigned = model.Where(x => x.IsSelected).Select(y => y.RoleName).ToList();
            if(RolesToBeAssigned.Any()){
                result = await _userManager.AddToRolesAsync(user, RolesToBeAssigned);
                if(!result.Succeeded) {
                    ModelState.AddModelError("", "Hubo un problema al agregar los roles al usuario.");
                    return View(model);
                }
            }
            return RedirectToAction("EditUser", new { id = id });
        }

        [HttpGet]
        public async Task<IActionResult> ManageUserClaims(string id){
            // Obtener el usuario de la base de datos por su ID
            var user = await _userManager.FindByIdAsync(id);
            if(user == null) { // El usuario no fue encontrado
                ViewBag.ErrorMessage = $"El usuario con el ID: {id} no fue encontrado.";
                return View("NotFound");
            }
            // Crear un modelo de vista para mostrar los claims del usuario
            ViewBag.UserName = user.UserName;
            var model = new UserClaimsViewModel {
                UserId = user.Id,
            };
            // Agregar todos los claims a la lista de claims
            var existingUserClaims = await _userManager.GetClaimsAsync(user);
            foreach(Claim claim in ClaimsStore.GetAllClaims()){
                // Crear un modelo de vista para cada claim
                UserClaim userClaim = new UserClaim {ClaimType = claim.Type};
                if(existingUserClaims.Any(c => c.Type == claim.Type)){userClaim.IsSelected = true;}
                model.Claims.Add(userClaim);
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ManageUserClaims(UserClaimsViewModel model, string id){
            // Obtener el usuario de la base de datos por su ID
            var user = await _userManager.FindByIdAsync(id);
            if(user == null) { // El usuario no fue encontrado
                ViewBag.ErrorMessage = $"El usuario con el ID: {id} no fue encontrado.";
                return View("NotFound");
            }
            // Eliminar todos los claims del usuario actual
            var claims = await _userManager.GetClaimsAsync(user);
            var result = await _userManager.RemoveClaimsAsync(user, claims);

            if (!result.Succeeded){
                ModelState.AddModelError("", "Hubo un problema al eliminar los claims del usuario.");
                return View(model);
            }
            // Agregar los calims seleccionados al usuario
            var AllSelectedClaims = model.Claims.Where(c => c.IsSelected)
                                         .Select(c => new Claim(c.ClaimType, c.ClaimType))
                                         .ToList();
            if(AllSelectedClaims.Any()){
                result = await _userManager.AddClaimsAsync(user, AllSelectedClaims);
                if(!result.Succeeded){
                    ModelState.AddModelError("", "Hubo un problema al agregar los claims al usuario.");
                    return View(model);
                }
            }
            return RedirectToAction("EditUser", new {id = id});
        }

        [HttpGet]
        public async Task<IActionResult> ManageRoleClaims(string id){
            var role = await _roleManager.FindByIdAsync(id);
            if(role == null) { // El rol no fue encontrado
                ViewBag.ErrorMessage = $"El rol con el ID: {id} no fue encontrado.";
                return View("NotFound");
            }
            // Crear un modelo de vista para mostrar los claims del rol
            ViewBag.RoleName = role.Name;
            var model = new RoleClaimsViewModel {
                RoleId = role.Id,
            };
            // Agregar todos los claims a la lista de claims
            var existingRoleClaims = await _roleManager.GetClaimsAsync(role);
            foreach(Claim claim in ClaimsStore.GetAllClaims()){
                // Crear un modelo de vista para cada claim en el rol actualizado
                RoleClaim roleClaim = new RoleClaim {ClaimType = claim.Type};
                // Verificar si el rol tiene este claim y seleccionarlo
                if(existingRoleClaims.Any(c => c.Type == claim.Type)){
                    roleClaim.IsSelected = true;
                }
                // Agregar el modelo de vista para cada claim
                model.Claims.Add(roleClaim);
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ManageRoleClaims(RoleClaimsViewModel model, string id){
            var role = await _roleManager.FindByIdAsync(id);
            if(role == null) { // El rol no fue encontrado
                ViewBag.ErrorMessage = $"El rol con el ID: {id} no fue encontrado.";
                return View("NotFound");
            }
            // Obtener todos los claims del rol
            var claims = await _roleManager.GetClaimsAsync(role);
            // Eliminar todos los claims del rol y agregar los nuevos claims
            for (var i = 0; i < model.Claims.Count; i++) {
                Claim claim = new Claim(model.Claims[i].ClaimType, model.Claims[i].ClaimType);
                IdentityResult? result;
                // Agregar el claim al rol si es necesario
                if(model.Claims[i].IsSelected && !(claims.Any(c => c.Type == claim.Type))){
                    result = await _roleManager.AddClaimAsync(role, claim);
                }else if(!model.Claims[i].IsSelected && claims.Any(c => c.Type == claim.Type)){
                    result = await _roleManager.RemoveClaimAsync(role, claim);
                }else { continue; }
                // Mostrar un mensaje de error si hubo un problema al agregar o eliminar el claim del rol
                if(result.Succeeded){
                    if(i < (model.Claims.Count - 1)){continue;}
                    else { return RedirectToAction("EditRole", new { id = id }); }
                }else{
                    ModelState.AddModelError("", "Hubo un problema al agregar o eliminar los claims del rol.");
                    return View(model);
                }
            }
            return RedirectToAction("EditRole", new { id = id });
        }
    }
}