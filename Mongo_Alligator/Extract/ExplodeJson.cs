﻿using System;
using System.Collections.Generic;
using System.Linq;
using Grasshopper.Kernel;
using System.Collections;
using System.Windows.Forms;
using BH.UI.Alligator.Base;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using BH.oM.Base;
using BH.oM.Geometry;
using BH.UI.Alligator.Query;

namespace BH.UI.Alligator.Mongo
{
    public class ExplodeJson : GH_Component, IGH_VariableParameterComponent
    {
        public ExplodeJson() : base("ExplodeJson", "ExplodeJson", "Explode json string into objects", "Alligator", "Mongo") { }
        public override Guid ComponentGuid { get { return new Guid("020CF2C8-CB67-4731-9CCA-50F0932E18DC"); } }
        protected override System.Drawing.Bitmap Internal_Icon_24x24 { get { return Properties.Resources.BHoM_Mongo_FromJson; } }
        public bool additional { get; set; }

        public bool CanInsertParameter(GH_ParameterSide side, int index)
        {
            return false;
        }
        public bool CanRemoveParameter(GH_ParameterSide side, int index)
        {
            return false;
        }
        public IGH_Param CreateParameter(GH_ParameterSide side, int index)
        {
            return new Grasshopper.Kernel.Parameters.Param_GenericObject();
        }
        public bool DestroyParameter(GH_ParameterSide side, int index)
        {
            return true;
        }
        public void VariableParameterMaintenance()
        {

        }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("json", "json", "json object", GH_ParamAccess.item);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            string json = "";
            json = DA.BH_GetData(0, json);
            if (json == null) return;

            BsonDocument bJson = BsonDocument.Parse(json);
            CustomObject customData = BsonSerializer.Deserialize<CustomObject>(bJson);
            m_Outputs = customData.CustomData;
            if (additional)
            {
                m_Outputs.Add("BHoM_Guid", customData.BHoM_Guid);
                m_Outputs.Add("Name", customData.Name);
                m_Outputs.Add("Tags", customData.Tags);
            }

            List<string> keys = m_Outputs.Keys.ToList();
            if (keys.Count == Params.Output.Count)
            {
                for (int i = 0; i < keys.Count; i++)
                {
                    var val = m_Outputs[keys[i]];
                    if (typeof(List<object>).IsAssignableFrom(val.GetType()))
                        DA.BH_SetDataList(i, val as List<object>);
                    else
                        DA.BH_SetData(i, val);
                }
            }
            else
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Remark, "The outputs need to be updated first. Please right-click on component and select update.");
            }
        }

        protected override void AppendAdditionalComponentMenuItems(ToolStripDropDown menu)
        {
            base.AppendAdditionalComponentMenuItems(menu);
            Menu_AppendItem(menu, "Update Outputs", Menu_DoClick);
            Menu_AppendItem(menu, "Append additional data", Menu_SetTrue);
        }
        private void Menu_SetTrue(object sender, EventArgs e)
        {
            if (additional) { additional = false; }
            else { additional = false; }
            UpdateOutputs();
        }

        private void Menu_DoClick(object sender, EventArgs e)
        {
            UpdateOutputs();
        }

        protected override void AfterSolveInstance()
        {
            UpdateOutputs();
        }
        private void UpdateOutputs()
        {
            List<string> keys = m_Outputs.Keys.ToList();

            int nbNew = keys.Count();
            int nbOld = Params.Output.Count();

            for (int i = 0; i < Math.Min(nbNew, nbOld); i++)
            {
                Params.Output[i].NickName = keys[i];
            }

            for (int i = nbOld - 1; i > nbNew - 1; i--)
                Params.UnregisterOutputParameter(Params.Output[i]);

            for (int i = nbOld; i < nbNew; i++)
            {
                if (typeof(IBHoMGeometry).IsAssignableFrom(m_Outputs[keys[i]].GetType()) ||
                    typeof(List<IBHoMGeometry>).IsAssignableFrom(m_Outputs[keys[i]].GetType()))
                {
                    BHoMGeometryParameter newParam = new BHoMGeometryParameter();
                    newParam.NickName = keys[i];
                    Params.RegisterOutputParam(newParam);
                }
                else if (typeof(IObject).IsAssignableFrom(m_Outputs[keys[i]].GetType()) ||
                         typeof(List<IObject>).IsAssignableFrom(m_Outputs[keys[i]].GetType()))
                {
                    BHoMObjectParameter newParam = new BHoMObjectParameter();
                    newParam.NickName = keys[i];
                    Params.RegisterOutputParam(newParam);
                }
                else
                {
                    Grasshopper.Kernel.Parameters.Param_GenericObject newParam = new Grasshopper.Kernel.Parameters.Param_GenericObject();
                    newParam.NickName = keys[i];
                    Params.RegisterOutputParam(newParam);
                }
            }
            this.OnAttributesChanged();
            if (nbNew != nbOld)
            {
                ExpireSolution(true);
            }
        }
        private Dictionary<string, object> m_Outputs = new Dictionary<string, object>();
    }
}
