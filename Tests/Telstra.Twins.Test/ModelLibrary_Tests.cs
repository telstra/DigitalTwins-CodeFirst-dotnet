using Telstra.Twins.Services;
using Xunit;

namespace Telstra.Twins.Test
{
    public class ModelLibrary_Tests
    {
        private readonly ModelLibrary _modelLibrary;

        public ModelLibrary_Tests()
        {
            _modelLibrary = new ModelLibrary();
        }

        [Fact]
        public void TwinModel_Should_Work()
        {
            // arrange
            var typeToAnalyze = typeof(Building);

            // act
            var twinModel = _modelLibrary.GetTwinModel(typeToAnalyze);

            // assert
            Assert.NotNull(twinModel);
            Assert.Equal(2, twinModel.Relationships.Count);

            Assert.Collection(twinModel.Relationships,
                r => Assert.True(r.Key.Name.Equals(nameof(Building.Floors)) && r.Value.Name.Equals("contains")),
                r => Assert.True(r.Key.Name.Equals(nameof(Building.OtherSpaces)) && r.Value.Name.Equals("otherPlaces")));
        }
    }
}
