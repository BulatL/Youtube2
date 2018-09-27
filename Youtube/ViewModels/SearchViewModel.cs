using System.Collections.Generic;

namespace Youtube.ViewModels
{
    public class SearchViewModel
    {
        public IEnumerable<CommentDTO> Comments { get; set; }
        public IEnumerable<VideoDTO> Videos { get; set; }
        public IEnumerable<UserForAdminPage> Users { get; set; }
         

    }
}
