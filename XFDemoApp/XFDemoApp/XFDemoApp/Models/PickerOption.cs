using System;
using System.Collections.Generic;
using System.Text;

namespace XFDemoApp.Models
{
    
    public class PickerOption<T>
    {
        public T Key { get; set; }
        public string Value { get; set; }

        public PickerOption() { }
        public PickerOption(T key, string value) { Key = key; Value = value; }
    }
}
