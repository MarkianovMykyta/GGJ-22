
using Characters;

namespace Souls
{
    public class Soul
    {
        public readonly Character Parent;

        public Soul(Character parent)
        {
            Parent = parent;
        }
    }
}