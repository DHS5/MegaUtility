using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Dhs5.Utility.DirectoryPicker
{
    [Serializable]
    public class DirectoryPicker
    {
        [SerializeField] private string path = "Assets/";

        public string Path => path;

        public void SetPath(string _path)
        {
            if (_path.StartsWith("Assets/")) path = _path;
        }
    }
}
