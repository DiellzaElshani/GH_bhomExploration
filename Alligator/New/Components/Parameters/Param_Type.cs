﻿using Grasshopper.Kernel;
using System;
using System.Collections.Generic;

namespace BH.UI.Alligator.Objects
{
    public class Param_Type : GH_PersistentParam<Engine.Alligator.Objects.GH_Type>
    {
        /*******************************************/
        /**** Properties                        ****/
        /*******************************************/

        protected override System.Drawing.Bitmap Icon { get; } = null;

        public override GH_Exposure Exposure { get; } = GH_Exposure.tertiary;

        public override Guid ComponentGuid { get; } = new Guid("AA7DDCDC-2789-4A23-88AD-E1E4CD84FB37");

        public override string TypeName { get; } = "Type";

        public bool Hidden { get; set; } = false;

        public bool IsPreviewCapable { get; } = false;


        /*******************************************/
        /**** Constructors                      ****/
        /*******************************************/

        public Param_Type()
            : base(new GH_InstanceDescription("Object Type 2", "Type", "Represents the type of an object", "Params", "Primitive"))
        {
        }


        /*******************************************/
        /**** Override Methods                  ****/
        /*******************************************/

        protected override GH_GetterResult Prompt_Singular(ref Engine.Alligator.Objects.GH_Type value)
        {
            return GH_GetterResult.cancel;
        }

        /*******************************************/

        protected override GH_GetterResult Prompt_Plural(ref List<Engine.Alligator.Objects.GH_Type> values)
        {
            return GH_GetterResult.cancel;
        }

        /*******************************************/
    }
}