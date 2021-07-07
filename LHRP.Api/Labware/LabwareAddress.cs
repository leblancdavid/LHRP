using LHRP.Api.Devices;
using System;
using System.Linq;

namespace LHRP.Api.Labware
{
  public class LabwareAddress : IEquatable<LabwareAddress>
    {
        public int Row { get; private set; }
        public int Column { get; private set; }
        public int PositionId { get; set; }

        public LabwareAddress(int row, int column)
        {
            Row = row;
            Column = column;
            PositionId = 0;
        }

        public LabwareAddress(int row, int column, int positionId)
        {
            Row = row;
            Column = column;
            PositionId = positionId;
        }

        public LabwareAddress(string alphaAddress, int positionId)
        {
            if (string.IsNullOrEmpty(alphaAddress))
            {
                Row = 0;
                Column = 0;
            }

            char alpha = alphaAddress.ElementAt(0);
            if (!Char.IsLetter(alpha))
            {
                Row = 0;
                Column = 0;
            }

            Row = (int)alpha - 64; 
            var columnString = alphaAddress.Substring(1, alphaAddress.Length - 1);
            int columnInt;
            var didParse = int.TryParse(columnString, out columnInt);
            if (!didParse)
            {
                Row = 0;
                Column = 0;
            }
            Column = columnInt;

            PositionId = positionId;
        }

        public bool Equals(LabwareAddress? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return other.Row == Row && other.Column == Column && other.PositionId == PositionId;
        }

        public override bool Equals(object? obj)
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