﻿using Beng.Specta.Compta.SharedKernel;

namespace Beng.Specta.Compta.Core.ValueTypes.Ids
{
    public class BaseLongPOId : BasePOId<long>
    {
        public BaseLongPOId(long value) : base(value)
        {
        }

        public static implicit operator BaseLongPOId(long value)
        {
            return new BaseLongPOId(value);
        }

        public static implicit operator BaseLongPOId(int value)
        {
            return new BaseLongPOId(value);
        }
    }

    public class ProjectId : BaseLongPOId
    {
        public ProjectId(long value) : base(value)
        {
        }
    }

    public class FundingGroupId : BaseLongPOId
    {
        public FundingGroupId(long value) : base(value)
        {
        }
    }

    public class FundingItemId : BaseLongPOId
    {
        public FundingItemId(long value) : base(value)
        {
        }
    }
}
