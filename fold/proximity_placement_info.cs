using System;
using System.Drawing;
using Grasshopper.Kernel;

namespace proximity_placement
{
    public class template_info : GH_AssemblyInfo
    {
        public override string Name
        {
            get
            {
                return "";
            }
        }
        public override Bitmap Icon
        {
            get
            {
                //Return a 24x24 pixel bitmap to represent this GHA library.
                return proximity_placement.Properties.Resources.enron;
            }
        }
        public override string Description
        {
            get
            {
                //Return a short string describing the purpose of this GHA library.
                return "";
            }
        }
        public override Guid Id
        {
            get
            {
                return new Guid("5263F29A-B2CC-4D9E-939F-999F723B9BA2"); // Tools -> Create Guid 5.
            }
        }
        public override string AuthorName
        {
            get
            {
                //Return a string identifying you or your company.
                return "Philipp Siedler";
            }
        }
        public override string AuthorContact
        {
            get
            {
                //Return a string representing your preferred contact details.
                return "p.d.siedler@gmail.com";
            }
        }
    }
}
