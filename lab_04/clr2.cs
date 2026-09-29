using System;
using System.Collections.Generic;
using System.Data;
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
    private HashSet<string> seen;
    private StringBuilder intermediateResult;

    public void Init()
    {
        seen = new HashSet<string>();
        intermediateResult = new StringBuilder();
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
        return new SqlString(this.intermediateResult.ToString());
    }

    public void Read(BinaryReader r)
    {
        this.seen = new HashSet<string>();
        this.intermediateResult = new StringBuilder();

        int count = r.ReadInt32();
        for (int i = 0; i < count; ++i)
        {
            string s = r.ReadString();
            AddDiagnosis(s);
        }
    }

    public void Write(BinaryWriter w)
    {
        w.Write(this.seen.Count);
        foreach (string s in this.seen)
            w.Write(s);
    }

    private void AddDiagnosis(string s)
    {
        if (this.seen.Add(s))
        {
            if (this.intermediateResult.Length > 0)
                this.intermediateResult.Append(", ");
            this.intermediateResult.Append(s);
        }
    }
}