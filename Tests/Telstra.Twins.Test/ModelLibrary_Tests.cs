using System.Linq;
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

        [Fact]
        public void TwinModelWithAbstractProperty_Should_Not_List_AbstractProperty_Twice()
        {
            // arrange
            var typeToAnalyze = typeof(BuildingWithAbstractProperty);

            // act
            var twinModel = _modelLibrary.GetTwinModel(typeToAnalyze);

            // assert
            var contentsCount = twinModel
                .contents
                .GroupBy(c => c.Name)
                .All(g => g.Count() == 1);
            Assert.True(contentsCount);
        }

        [Fact]
        public void TwinModelFactory_TwinModelWithAbstractProperty_Should_Not_List_AbstractProperty_Twice()
        {
            // arrange
            var typeToAnalyze = typeof(BuildingWithAbstractProperty);
            var factory = new TwinModelFactory();
            // act
            var twinModel = factory.CreateTwinModel(typeToAnalyze);

            // assert
            var contentsCount = twinModel
                .contents
                .GroupBy(c => c.Name)
                .All(g => g.Count() == 1);
            Assert.True(contentsCount);
        }
    }
}
