using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;

using System.Linq;

//
using Rhino;
//using Rhino.Geometry;
using Rhino.DocObjects;
using Rhino.Collections;

using GH_IO;
using GH_IO.Serialization;
using Grasshopper;
//using Grasshopper.Kernel;
using Grasshopper.Kernel.Data;
using Grasshopper.Kernel.Types;

//using System;
using System.IO;
//using System.Xml;
//using System.Xml.Linq;
//using System.Linq;
//using System.Data;
using System.Drawing;
using System.Reflection;
using System.Collections;
using System.Windows.Forms;
//using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace proximity_placement

{
    public class vanillaComponent : GH_Component
    {
        //GH_Document GrasshopperDocument;
        //IGH_Component Component;
        
        /// <summary>
        /// Each implementation of GH_Component must provide a public 
        /// constructor without any arguments.
        /// Category represents the Tab in which the component will appear, 
        /// Subcategory the panel. If you use non-existing tab or panel names, 
        /// new tabs/panels will automatically be created.
        /// </summary>
        public vanillaComponent(): base("proximity_placement", "description", "action description", "philsComp", "00generation")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {

            pManager.AddTextParameter("enron", "enron", "enron", GH_ParamAccess.list);
            pManager.AddIntegerParameter("index", "index", "index", GH_ParamAccess.list);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("name", "name", "name", GH_ParamAccess.list);
            pManager.AddIntegerParameter("in_count", "in_count", "in_count", GH_ParamAccess.list);
            pManager.AddIntegerParameter("out_count", "out_count", "out_count", GH_ParamAccess.list);
            pManager.AddTextParameter("source", "source", "source", GH_ParamAccess.list);
            pManager.AddTextParameter("target", "target", "target", GH_ParamAccess.list);
            pManager.AddTextParameter("most_influence_name", "most_influence_name", "most_influence_name", GH_ParamAccess.list);
            pManager.AddIntegerParameter("most_influence_index", "most_influence_index", "most_influence_index", GH_ParamAccess.list);
            pManager.AddIntegerParameter("most_influence_count", "most_influence_count", "most_influence_count", GH_ParamAccess.list);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object can be used to retrieve data from input parameters and 
        /// to store data in output parameters.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {

            List<string> enron = new List<string>();
            DA.GetDataList(0, enron);
            List<int> index = new List<int>();
            DA.GetDataList(1, index);

            var enronClass = new pp_class(enron, index);
            
            DA.SetDataList(0, enronClass.dictionaryList);
            DA.SetDataList(1, enronClass.incoming);
            DA.SetDataList(2, enronClass.outgoing);
            DA.SetDataList(3, enronClass.source);
            DA.SetDataList(4, enronClass.target);
            DA.SetDataList(5, enronClass.mIN);
            DA.SetDataList(6, enronClass.mII);
            DA.SetDataList(7, enronClass.iC);
        }

        /// <summary>
        /// Provides an Icon for every component that will be visible in the User Interface.
        /// Icons need to be 24x24 pixels.
        /// </summary>
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                // You can add image files to your project resources and access them like this:
                //return Resources.IconForThisComponent;
                return proximity_placement.Properties.Resources.enron;
            }
        }

        /// <summary>
        /// Each component must have a unique Guid to identify it. 
        /// It is vital this Guid doesn't change otherwise old ghx files 
        /// that use the old ID will partially fail during loading.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("5263F29A-B2CC-4D9E-939F-999F723B9BA2"); }
        }
        
    }
}
