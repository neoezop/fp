﻿using System.Windows.Forms;

namespace TagCloud
{
    public class SaveImageAction : IUiAction
    {
        private readonly IImageHolder imageHolder;

        public MenuCategory Category => MenuCategory.File;
        public string Name => "Save";
        public string Description => "Save image into file";

        public SaveImageAction(IImageHolder imageHolder)
        {
            this.imageHolder = imageHolder;
        }

        public void Perform()
        {
            var dialog = new SaveFileDialog
            {
                CheckFileExists = false,
                InitialDirectory = HelperMethods.GetProjectDirectory().GetValueOrThrow(),
                DefaultExt = "Png",
                FileName = "TagCloud.Png"
            };
            var res = dialog.ShowDialog();
            if (res == DialogResult.OK)
                imageHolder.SaveImage(dialog.FileName);
        }
    }
}
