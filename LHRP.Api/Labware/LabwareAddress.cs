using LHRP.Api.Devices;
using System;

namespace LHRP.Api.Labware
{
  public class LabwareAddress : IEquatable<LabwareAddress>
    {
        public int Row { get; private set; }
        public int Column { get; private set; }
        public int PositionId { get; set; }

        public LabwareAddress(int row, int column, int positionId)
        {
            Row = row;
            Column = column;
            PositionId = positionId;
        }

        public bool Equals(LabwareAddress other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return other.Row == Row && other.Column == Column && other.PositionId == PositionId;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof (LabwareAddress)) return false;
            return Equals((LabwareAddress) obj);
        }

        public static bool operator ==(LabwareAddress obj1, LabwareAddress obj2)
        {
            if (ReferenceEquals(obj1, obj2))
            {
                return true;
            }

            if (ReferenceEquals(obj1, null))
            {
                return false;
            }
            if (ReferenceEquals(obj2, null))
            {
                return false;
            }

            return obj1.Equals(obj2);
        }

        // this is second one '!='
        public static bool operator !=(LabwareAddress obj1, LabwareAddress obj2)
        {
            return !(obj1 == obj2);
        }

        public string ToAlphaAddress()
        {
            return $"{(char)(Row + 64)}{Column}";
        }

        public override int GetHashCode()
        {
            return 0;
            // unchecked
            // {
            //     int result = Row;
            //     result = (result*397) ^ Column;
            //     result = (result*397) ^ PositionId;
            //     return result;
            // }
        }
    }
}