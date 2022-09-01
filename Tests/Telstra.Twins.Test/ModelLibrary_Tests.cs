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

        [Fact]
        public void TwinModelFactory_ExtendingBuilding_Should_Not_List_BaseProperties()
        {
            // arrange
            var typeToAnalyze = typeof(ExtendedBuildingWithAbstractParent);
            var factory = new TwinModelFactory();

            var twinModelFromBase = factory.CreateTwinModel<BuildingWithAbstractProperty>();

            // act
            var twinModel = factory.CreateTwinModel(typeToAnalyze);

            // assert
            var baseModelContents = twinModelFromBase.contents.Select(x => x.Name).ToList();
            var extendedModelContents = twinModel.contents.Select(x => x.Name).ToList();

            var containsBaseContents = extendedModelContents
                .Any(e => baseModelContents.Contains(e));
            Assert.False(containsBaseContents);
        }

        [Fact]
        public void ExtendingRelationships_ExtendingBuilding_Should_List_BaseRelationships()
        {
            // arrange
            var typeToAnalyze = typeof(ExtendedBuilding);

            // act
            var twinModel = _modelLibrary.GetTwinModel(typeToAnalyze);

            // assert
            Assert.Equal(2, twinModel.ExtendingRelationships.Count);
        }

        [Fact]
        public void ExtendingRelationships_DoubleExtendedBuilding_Should_List_AllBaseRelationships()
        {
            // arrange
            var typeToAnalyze = typeof(DoubleExtendedBuilding);

            // act
            var twinModel = _modelLibrary.GetTwinModel(typeToAnalyze);

            // assert
            Assert.Equal(3, twinModel.ExtendingRelationships.Count);
        }

    }
}
