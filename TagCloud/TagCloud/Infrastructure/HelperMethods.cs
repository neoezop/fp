﻿using ResultOF;
using System;
using System.IO;

namespace TagCloud
{
    public static class HelperMethods
    {
        public static Result<string> GetProjectDirectory()
        {
            string workingDirectory = Environment.CurrentDirectory;
            string projectDirectory = Directory.GetParent(workingDirectory).Parent.FullName;
            return projectDirectory;
        }
    }
}
