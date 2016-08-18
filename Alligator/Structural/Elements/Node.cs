﻿using BHoM.Structural;
using System;
using Grasshopper.Kernel;
using System.Collections.Generic;
using Alligator.Components;
using GHE = Grasshopper_Engine;
using BHE = BHoM.Structural.Elements;
using BHI = BHoM.Structural.Interface;
using Rhino.Geometry;
using Grasshopper;

namespace Alligator.Structural.Elements
{
    public class CreateNode : BHoMBaseComponent<BHE.Node>
    {
        public CreateNode() : base("Create Node", "CreateNode", "Create a BH Node object", "Structure", "Elements") { }

        public override Guid ComponentGuid
        {
            get
            {
                return new Guid("5FE0E2C4-5E50-410F-BBC7-C255FD1BD2B3");
            }
        }

        /// <summary> Icon (24x24 pixels)</summary>
        protected override System.Drawing.Bitmap Internal_Icon_24x24
        {
            get { return Alligator.Properties.Resources.node; }
        }
    }

    public class MultiExportNode : GH_Component
    {
        public MultiExportNode() : base("Multi Export Node", "ExNode", "Creates or Replaces the geometry of a Node", "Structure", "Elements") { }

        public override GH_Exposure Exposure
        {
            get
            {
                return GH_Exposure.secondary;
            }
        }

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Application", "App", "Application to import nodes from", GH_ParamAccess.item);
            pManager.AddGenericParameter("Nodes", "N", "BHoM nodes to export", GH_ParamAccess.list);
            pManager.AddBooleanParameter("Execute", "R", "Generate Nodes", GH_ParamAccess.item);

            pManager[2].Optional = true;
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddIntegerParameter("Ids", "Ids", "Node Numbers", GH_ParamAccess.list);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            if (GHE.DataUtils.Run(DA, 2))
            {
                BHI.IElementAdapter app = GHE.DataUtils.GetGenericData<BHI.IElementAdapter>(DA, 0);
                if (app != null)
                {
                    List<BHE.Node> nodes = GHE.DataUtils.GetGenericDataList<BHE.Node>(DA, 1);
                    List<string> ids = null;
                    app.SetNodes(nodes, out ids);

                    DA.SetDataList(0, ids);
                }
            }
        }

        public override Guid ComponentGuid
        {
            get { return new Guid("c811c998-a60f-4015-8bed-a79d22467a20"); }
        }

        /// <summary> Icon (24x24 pixels)</summary>
        protected override System.Drawing.Bitmap Internal_Icon_24x24
        {
            get { return Alligator.Properties.Resources.node; }
        }
    }

    public class MultiImportNode : ImportComponent
    {
        public MultiImportNode() : base("Multi Import Node", "GetNode", "Get the geometry and properties of a node", "Structure", "Elements")
        {

        }

        public override GH_Exposure Exposure
        {
            get
            {
                return GH_Exposure.tertiary;
            }
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            if (GHE.DataUtils.Run(DA, 2))
            {
                BHI.IElementAdapter app = GHE.DataUtils.GetGenericData<BHI.IElementAdapter>(DA, 0);
                if (app != null)
                {
                    List<string> ids = null;
                    List<BHE.Node> nodes = null;
                    DataTree<Point3d> locations = new DataTree<Point3d>();
                    if (m_Selection == BHI.ObjectSelection.FromInput)
                        ids = GHE.DataUtils.GetDataList<string>(DA, 1);

                    app.Selection = m_Selection;
                    ids = app.GetNodes(out nodes, ids);

                    foreach (BHE.Node node in nodes)
                    {
                        locations.Add(GHE.GeometryUtils.Convert(node.Point));
                    }
                    DA.SetDataList(0, ids);
                    DA.SetDataList(1, nodes);
                    DA.SetDataTree(2, locations);
                }
            }
        }

        public override Guid ComponentGuid
        {
            get { return new Guid("{7E9F8C98-606D-4BC2-80EF-2AAE3C1CEB76}"); }
        }

        /// <summary> Icon (24x24 pixels)</summary>
        protected override System.Drawing.Bitmap Internal_Icon_24x24
        {
            get { return Alligator.Properties.Resources.node; }
        }
    }
}