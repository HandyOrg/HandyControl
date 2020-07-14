using System;
using System.Windows.Data;

namespace HandyControl.Data
{
    public class EnumDataProvider : ObjectDataProvider
    {
        private Type _type;
        public Type Type
        {
            get => _type;
            set
            {
                _type = value;
                MethodName = "GetValues";
                ObjectType = typeof(System.Enum);
                MethodParameters.Add(value);
            }
        }
    }
}
