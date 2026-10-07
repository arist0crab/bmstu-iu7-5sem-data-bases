using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.IO;
using System.Text;
using Microsoft.SqlServer.Server;

[Serializable]
[SqlUserDefinedAggregate(
    Format.UserDefined,
    IsInvariantToNulls = true,
    IsInvariantToDuplicates = true,
    IsInvariantToOrder = false,
    MaxByteSize = -1)
]
public class DiagnosesAggregation : IBinarySerialize
{
    private List<string> seen;

    public void Init()
    {
        seen = new List<string>();
    }

    public void Accumulate(SqlString value)
    {
        if (value.IsNull || string.IsNullOrWhiteSpace(value.Value))
            return;

        string diagnosis = value.Value.Trim();
        AddDiagnosis(diagnosis);
    }

    public void Merge(DiagnosesAggregation other)
    {
        if (other == null || other.seen == null)
            return;

        foreach (string s in other.seen)
            AddDiagnosis(s);
    }

    public SqlString Terminate()
    {
        if (seen == null || seen.Count == 0)
            return SqlString.Null;

        return new SqlString(string.Join(", ", seen));
    }

    public void Read(BinaryReader r)
    {
        this.seen = new List<string>();

        int count = r.ReadInt32();
        for (int i = 0; i < count; ++i)
        {
            this.seen.Add(r.ReadString());
        }
    }

    public void Write(BinaryWriter w)
    {
        if (this.seen == null)
        {
            w.Write(0);
            return;
        }

        w.Write(this.seen.Count);
        foreach (string s in this.seen)
        {
            w.Write(s);
        }
    }

    private void AddDiagnosis(string s)
    {
        if (!this.seen.Contains(s))
        {
            this.seen.Add(s);
        }
    }
}