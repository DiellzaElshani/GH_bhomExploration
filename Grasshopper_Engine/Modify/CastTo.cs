﻿/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2018, the respective contributors. All rights reserved.
 *
 * Each contributor holds copyright over their respective contributions.
 * The project versioning (Git) records all such contribution source information.
 *                                           
 *                                                                              
 * The BHoM is free software: you can redistribute it and/or modify         
 * it under the terms of the GNU Lesser General Public License as published by  
 * the Free Software Foundation, either version 3.0 of the License, or          
 * (at your option) any later version.                                          
 *                                                                              
 * The BHoM is distributed in the hope that it will be useful,              
 * but WITHOUT ANY WARRANTY; without even the implied warranty of               
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the                 
 * GNU Lesser General Public License for more details.                          
 *                                                                            
 * You should have received a copy of the GNU Lesser General Public License     
 * along with this code. If not, see <https://www.gnu.org/licenses/lgpl-3.0.html>.      
 */

using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;

namespace BH.Engine.Grasshopper
{
    public static partial class Modify
    {
        /*******************************************/
        /**** Public Methods                    ****/
        /*******************************************/

        public static bool CastTo<Q>(object value, ref Q target)
        {
            if (value == null)
                target = default(Q);

            if (value is Q)
                return true;

            if (target is GH_Vector)
            {
                GH_Vector vector = null;
                GH_Convert.ToGHVector(value, GH_Conversion.Both, ref vector);
                target = (Q)(object)vector;
            }
            else if (target is GH_Curve)
            {
                GH_Curve curve = null;
                GH_Convert.ToGHCurve(value, GH_Conversion.Both, ref curve);
                target = (Q)(object)curve;
            }
            else if (target is GH_Surface)
            {
                GH_Surface surface = null;
                GH_Convert.ToGHSurface(value, GH_Conversion.Both, ref surface);
                target = (Q)(object)surface;
            }
            else if (target is GH_Brep)
            {
                GH_Brep bRep = null;
                GH_Convert.ToGHBrep(value, GH_Conversion.Both, ref bRep);
                target = (Q)(object)bRep;
            }
            else if (target is GH_MeshFace)
            {
                GH_MeshFace face = null;
                GH_Convert.ToGHMeshFace(value, GH_Conversion.Both, ref face);
                target = (Q)(object)face;
            }
            else if (target is GH_Transform)
            {
                GH_Transform transform = new GH_Transform(value as dynamic);
                target = (Q)(object)transform;
            }
            else if (target is GH_Matrix)
            {
                GH_Matrix transform = new GH_Matrix(value as dynamic);
                target = (Q)(object)transform;
            }
            else if (target is IGH_GeometricGoo)
                target = (Q)GH_Convert.ToGeometricGoo(value);
            else
                target = (Q)(Convert.IToGoo(value));

            return true;
        }

        /*************************************/
    }
}