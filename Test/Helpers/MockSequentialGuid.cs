using System;
using Zaabee.SequentialGuid;

namespace Test.Helpers
{
    public static class MockSequentialGuid
    {
        public static Guid NewGuid() => SequentialGuidHelper.GenerateComb(SequentialGuidType.SequentialAtEnd);
    }
}
