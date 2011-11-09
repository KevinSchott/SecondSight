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

using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Data;

namespace SecondSight
{

/// <summary>
/// Comparer class for sorting report results when Group By is a Sphere or Add field
/// </summary>
public sealed class RxSortComparer : IComparer<KeyValuePair<object, DataRow>>
{
    public int Compare(KeyValuePair<object, DataRow> a, KeyValuePair<object, DataRow> b)
    {
        if (a.Equals(b))
            return 0;
        float af = (float)a.Key;
        float bf = (float)b.Key;

        return af.CompareTo(bf);
    }
};

/// <summary>
/// Comparer class for sorting report results when Group By is a Cylinder field
/// </summary>
public sealed class CylinderSortComparer : IComparer<KeyValuePair<object, DataRow>>
{
    public int Compare(KeyValuePair<object, DataRow> a, KeyValuePair<object, DataRow> b)
    {
        if (a.Equals(b))
            return 0;
        float af = (float)a.Key;
        float bf = (float)b.Key;

        return bf.CompareTo(af);
    }
};

/// <summary>
/// Comparer class for sorting report results when Group By is an Axis field.
/// </summary>
public sealed class AxisSortComparer : IComparer<KeyValuePair<object, DataRow>>
{
    public int Compare(KeyValuePair<object, DataRow> a, KeyValuePair<object, DataRow> b)
    {
        if (a.Equals(b))
            return 0;

        char ac = (char)a.Key;
        char bc = (char)b.Key;

        if (ac == 'W' || bc == 'O')
            return -1;
        else if (ac == 'O' || bc == 'W') 
            return 1;
        return 0;
    }
};

public sealed class TypeSortComparer : IComparer<KeyValuePair<object, DataRow>>
{
    public int Compare(KeyValuePair<object, DataRow> a, KeyValuePair<object, DataRow> b)
    {
        if (a.Equals(b))
            return 0;

        char ac = (char)a.Key;
        char bc = (char)b.Key;

        if (ac == 'S' || bc == 'M')
            return -1;

        return 1;
    }
}

public sealed class GenderSortComparer : IComparer<KeyValuePair<object, DataRow>>
{
    public int Compare(KeyValuePair<object, DataRow> a, KeyValuePair<object, DataRow> b)
    {
        if (a.Equals(b))
            return 0;

        char ac = (char)a.Key;
        char bc = (char)b.Key;

        if (ac == 'U' || bc == 'F')
            return -1;
        else if (ac == 'F' || bc == 'U')
            return 1;

        return 0;
    }
}

public sealed class SizeSortComparer : IComparer<KeyValuePair<object, DataRow>>
{
    public int Compare(KeyValuePair<object, DataRow> a, KeyValuePair<object, DataRow> b)
    {
        if (a.Equals(b))
            return 0;

        char ac = (char)a.Key;
        char bc = (char)b.Key;

        if (ac == 'S' || bc == 'L')
            return -1;
        else if (ac == 'L' || bc == 'S')
            return 1;

        return 0;
    }
}

public sealed class TintSortComparer : IComparer<KeyValuePair<object, DataRow>>
{
    public int Compare(KeyValuePair<object, DataRow> a, KeyValuePair<object, DataRow> b)
    {
        if (a.Equals(b))
            return 0;

        char ac = (char)a.Key;
        char bc = (char)b.Key;

        if (ac == 'N' || bc == 'D')
            return -1;
        else if (ac == 'D' || bc == 'N')
            return 1;

        return 0;
    }
}

} //namespace SecondSight