using System;

namespace FdinhaServer.Entities
{
    [Serializable]
    public struct Card
    {
        public bool Valid;
        public int Value;
        public Suit Suit;

        public static bool operator <(Card a, Card b)
        {
            if (a.Value == b.Value)
                return a.Suit < b.Suit;
            else
                return a.Value < b.Value;
        }

        public static bool operator >(Card a, Card b)
        {
            if (a.Value == b.Value)
                return a.Suit > b.Suit;
            else
                return a.Value > b.Value;
        }

        public static bool operator !=(Card a, Card b)
        {
            return a.Value != b.Value || a.Suit != b.Suit || a.Valid != b.Valid;
        }

        public static bool operator ==(Card a, Card b)
        {
            return a.Value == b.Value && a.Suit == b.Suit && a.Valid == b.Valid;
        }

        public static bool operator ==(Card a, Card? b)
        {
            if (b == null)
                return !a.Valid;
            return a == b.Value;
        }

        public static bool operator !=(Card a, Card? b)
        {
            if (b == null)
                return a.Valid;
            return a != b.Value;
        }

        public override bool Equals(object obj)
        {
            return obj is Card card &&
                   Valid == card.Valid &&
                   Value == card.Value &&
                   Suit == card.Suit;
        }

        public override int GetHashCode()
        {
            int hashCode = -2027428293;
            hashCode = hashCode * -1521134295 + Valid.GetHashCode();
            hashCode = hashCode * -1521134295 + Value.GetHashCode();
            hashCode = hashCode * -1521134295 + Suit.GetHashCode();
            return hashCode;
        }
    }
}
