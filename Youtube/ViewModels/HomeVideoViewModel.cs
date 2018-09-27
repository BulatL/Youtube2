using System.Collections.Generic;

namespace Youtube.ViewModels
{
    public class HomeVideoViewModel
    {
        public List<VideoDTO> Top5Videos { get; set; }
        public List<VideoDTO> AllVideos { get; set; }
    }
}
