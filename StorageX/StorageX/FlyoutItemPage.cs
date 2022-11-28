using System;
namespace StorageX.PageModel
{
    public class FlyoutItemPage
    {
        public FlyoutItemPage()
        {
        }
        public string Title { get; set; }
        public string IconSourse { get; set; }
        public Type TargetPage { get; set; }
    }
}

