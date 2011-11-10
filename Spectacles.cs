// Copyright 2009, 2010, 2011 Kevin Schott

// This file is part of SecondSight.

// SecondSight is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.

// SecondSight is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.

// You should have received a copy of the GNU General Public License
// along with SecondSight.  If not, see <http://www.gnu.org/licenses/>.

//Spectacles class
//Container class for a glasses prescription
using System;

namespace SecondSight
{
	#region Support Structs and Enums
	
	public enum DomEye {OD, OS, OU}
	
	public struct SpecType 
	{
		public const string Single = "S";
		public const string Multi = "M";
	}
	
    public struct SpecSize   
    { 
    	public const string Small = "S";
    	public const string Medium = "M";
    	public const string Large = "L";
    	public const string Child = "C";
    }
    
    public struct SpecTint   
    { 
    	public const string None = "N";
    	public const string Light = "L";
    	public const string Dark = "D";
    }
    
    public struct SpecGender 
    { 
    	public const string Male = "M";
    	public const string Female = "F";
    	public const string Uni = "U";
    }
	#endregion
    
    public class Spectacles
    {
        public float SphereOD { get; set; }
        public float SphereOS { get; set; }
        public float CylOD { get; set; }
        public float CylOS { get; set; }
        public int AxisOD { get; set; }
        public int AxisOS { get; set; }
        public float AddOD { get; set; }
        public float AddOS { get; set; }
        public string Type { get; set; }
        public string Gender { get; set; }
        public string Size { get; set; }
        public string Tint { get; set; }
        public string Comment { get; set; }

        //Constructor
        public Spectacles()
        {
            SphereOD = SphereOS = CylOD = CylOS = AddOD = AddOS = 0.0F;
            AxisOD = AxisOS = 0;
            Gender = SpecGender.Male;
            Tint = SpecTint.None;
            Type = SpecType.Single;
            Size = SpecSize.Medium;
            Comment = "";
        }
        public Spectacles(float sod, float sos, float cod, float cos, int axod, int axos, float aod, float aos,
            string gen, string tint, string type, string size)
        {
            SphereOD = sod;
            SphereOS = sos;
            CylOD = cod;
            CylOS = cos;
            AxisOD = axod;
            AxisOS = axos;
            AddOD = aod;
            AddOS = aos;
            Gender = gen;
            Tint = tint;
            Type = type;
            Size = size;
        }
    }

    //SpecsRecord class
    //Spectacles including database record informaiton
    public class SpecsRecord : Spectacles
    {
        public uint SKU { get; set; }
        public string DateAdded { get; set; }
        public string DateDispensed { get; set; }
    }
}
