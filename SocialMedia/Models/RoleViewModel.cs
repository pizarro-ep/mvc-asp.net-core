using SocialMedia.Models;

namespace SocialMedia.Models {
    public class RoleViewModel {
        public IEnumerable<ApplicationRole> Roles { get; set; } 
        public bool CanEdit { get; set; }
        public bool CanDelete { get; set; }

        public RoleViewModel() {
            Roles = Enumerable.Empty<ApplicationRole>(); // Inicializa Roles con una colección vacía
        }
    }
}