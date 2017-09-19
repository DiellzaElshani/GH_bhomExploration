﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grasshopper.Kernel;
using RG = Rhino.Geometry;
using BHG = BH.oM.Geometry;
using GHE = BH.Engine.Grasshopper;

namespace BH.UI.Alligator.Geometry
{
    public class Group : GH_Component
    {
        public Group() : base("BHGroup", "BHGroup", "Create a BHoM Group", "Alligator", "geometry") { }

        public override Guid ComponentGuid
        {
            get
            {
                return new Guid("8E80E222-5459-402A-BDB7-45FD1E3374B0");
            }
        }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddCurveParameter("curves", "curves", "GH curves", GH_ParamAccess.list);
        }
        protected override System.Drawing.Bitmap Internal_Icon_24x24
        {
            get { return Alligator.Geometry.Properties.Resources.BHoM_Geometry_Group; }
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Group", "Group", "BHoM Group", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            List<RG.Curve> rhinoCurves = GHE.DataUtils.GetDataList<RG.Curve>(DA, 0);

            BHG.GeometryGroup group = new BHG.GeometryGroup(rhinoCurves.Select(x => GHE.GeometryUtils.Convert(x)));

            //foreach ( RG.Curve curve in rhinoCurves)
            //{
            //    RG.Polyline inPolyline = null;
            //    bool working = curve.TryGetPolyline(out inPolyline);

            //    List<BHG.Point> bhPoints = new List<BHG.Point>();
            //    if (working)
            //    {
            //        foreach (RG.Point3d point in inPolyline)
            //            bhPoints.Add(new BHG.Point(point.X, point.Y, point.Z));
            //    }

            //    group.Add(new BHG.Polyline(bhPoints));
            //}

            DA.SetData(0, group);
        }
    }
}