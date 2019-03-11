using System;
using System.Collections.Generic;
using System.Text;

namespace Unicell.DTO
{
    public class AppMetadataComparer : IEqualityComparer<AppMetadataDTO>
    {
        public bool Equals(AppMetadataDTO app1, AppMetadataDTO app2)
        {
            return (app1.PackageName.Trim().ToUpper() == app2.PackageName.Trim().ToUpper());
        }

        public int GetHashCode(AppMetadataDTO obj)
        {
            return obj.PackageName.GetHashCode();
        }
    }
}
