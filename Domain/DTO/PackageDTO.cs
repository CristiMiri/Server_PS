using System;

namespace Server.Domain.DTO
{
    internal class PackageDTO
    {
        public bool Result { get; private set; }
        public string Message { get; private set; }
        public object Data { get; private set; }

        // Default constructor made private to enforce the use of the builder
        private PackageDTO() { }

        // Builder class
        public class Builder
        {
            private readonly PackageDTO _packageDTO;

            public Builder()
            {
                _packageDTO = new PackageDTO();
            }

            public Builder SetResult(bool result)
            {
                _packageDTO.Result = result;
                return this;
            }

            public Builder SetMessage(string message)
            {
                _packageDTO.Message = message;
                return this;
            }

            public Builder SetData(object data)
            {
                _packageDTO.Data = data;
                return this;
            }

            public PackageDTO Build()
            {
                return _packageDTO;
            }
        }
    }
}
