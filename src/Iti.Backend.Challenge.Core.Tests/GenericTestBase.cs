using AutoFixture;

namespace Iti.Backend.Challenge.Core.Tests
{

    /// <summary>
    /// Generic test abstract class
    /// </summary>
    public abstract class GenericTestBase
    {

        protected readonly IFixture _fixture;

        public GenericTestBase()
        {
            _fixture = new Fixture();
        }

        #region Helpers

        /// <summary>
        /// Create a mock instance
        /// </summary>
        /// <typeparam name="T">Object type</typeparam>
        protected T One<T>()
            => _fixture.Create<T>();

        #endregion

    }
}
