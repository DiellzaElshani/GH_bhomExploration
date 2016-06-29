﻿using System;
using System.Drawing;
using System.Collections.Generic;
using BHoM_Engine.ModelLaundry;
using BHoM.Geometry;
using Grasshopper.Kernel;
using Rhino.Geometry;
using BH = BHoM.Geometry;
using R = Rhino.Geometry;

namespace Alligator.ModelLaundry
{
    public class HorizontalPointSnap : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the HorizontalPointSnap class.
        /// </summary>
        public HorizontalPointSnap()
          : base("HorizontalPointSnap", "HPtSnap",
              "Horizontal point snapping",
              "Alligator", "ModelLaundry")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddCurveParameter("GeometryToSnap", "GeomToSnap", "Input an an BHoM polyline", GH_ParamAccess.item);
            pManager.AddGenericParameter("GeometryToSnapTo", "GeomToSnapTo", "Input an an BHoM Brep", GH_ParamAccess.list);
            pManager.AddNumberParameter("Tolerance", "Tol", "Set a tolerance for the snapping", GH_ParamAccess.item, 0.2);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Result", "res", "New BHoM Polyline", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            BH.Curve contour = Utils.GetGenericData<BH.Curve>(DA, 0);
            double tol = Utils.GetData<double>(DA, 2);
            List<BH.Curve> refContour = Utils.GetGenericDataList<BH.Curve>(DA, 1);

            for (int i = 0; i < refContour.Count; i++)
            {
                if (refContour[i] is BHoM.Geometry.PolyCurve)
                {
                    List<BH.Curve> tempCrv = refContour[i].Explode();
                    List<BH.Point> tempPts = new List<BH.Point>();
                    for (int j = 0; j < tempCrv.Count; j++)
                    {
                        tempPts.Add(tempCrv[j].StartPoint);
                    }

                    if (refContour[i].IsClosed())
                    {
                        //tempPts.Add(tempCrv(tempCrv.Count-1).)
                    }
                }

            }
        }

        /// <summary>
        /// Provides an Icon for the component.
        /// </summary>
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                //You can add image files to your project resources and access them like this:
                // return Resources.IconForThisComponent;
                return null;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("{ad59f890-c387-4a68-a2cc-bc401725779d}"); }
        }
    }
}