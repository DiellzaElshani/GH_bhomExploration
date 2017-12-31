﻿using System;
using Grasshopper.Kernel;
using MA = BH.Adapter.Mongo;

namespace Mongo_Alligator
{
    public class MoveCollection : GH_Component
    {
        public MoveCollection() : base("Move Collection", "MoveCollection", "Moves all the content from one collection to another. Overwrites the target collection", "Alligator", "Mongo") { }
        public override Guid ComponentGuid { get { return new Guid("929F2358-8CCA-4368-8E7D-97EAD50BB730"); } }
        public override GH_Exposure Exposure { get { return GH_Exposure.primary; } }

        protected override System.Drawing.Bitmap Internal_Icon_24x24 { get { return BH.UI.Alligator.Mongo.Properties.Resources.MoveCollection; } }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Source", "Source", "Source mongo link collection to move data FROM", GH_ParamAccess.item);
            pManager.AddGenericParameter("Target", "Target", "Target mongo link collection to move data TO", GH_ParamAccess.item);
            pManager.AddBooleanParameter("Overwrite", "Overwrite", "If set to true the content of the target collection will be overwritten if the target is non-empty", GH_ParamAccess.item);
            pManager.AddBooleanParameter("active", "active", "check if the component currently allows data transfer", GH_ParamAccess.item, false);

        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddBooleanParameter("Success", "Success", "Success", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            MA.MongoAdapter source = new BH.Adapter.Mongo.MongoAdapter();
            MA.MongoAdapter target = new BH.Adapter.Mongo.MongoAdapter();
            bool overwrite = false, active = false, success = false;
            DA.GetData(0, ref source);
            DA.GetData(1, ref target);
            DA.GetData(2, ref overwrite);
            DA.GetData(3, ref overwrite);

            if (active)
            {
                success = source.MoveCollection(target, overwrite);
            }
            DA.SetData(0, success);
        }
    }
}