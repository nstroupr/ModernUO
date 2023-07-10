using System;
using Server.Network;
using Xunit;

namespace Server.Tests.Network
{
    public class MapPatchesTests : IClassFixture<ServerFixture>
    {
        [Fact]
        public void TestMapPatches()
        {
            var ns = PacketTestUtilities.CreateTestNetState();

            var expected = new MapPatches().Compile();

            ns.SendMapPatches();

            var result = ns.SendPipe.Reader.TryRead();
            AssertThat.Equal(result.Buffer[0].AsSpan(0), expected);
        }

        [Fact]
        public void TestInvalidMapEnable()
        {
            var expected = new InvalidMapEnable().Compile();

            var ns = PacketTestUtilities.CreateTestNetState();
            ns.SendInvalidMap();

            var result = ns.SendPipe.Reader.TryRead();
            AssertThat.Equal(result.Buffer[0].AsSpan(0), expected);
        }

        [Theory]
        [InlineData("Gaia")]
        [InlineData("Malas")]
        public void TestMapChange(string mapName)
        {
            var map = Map.Parse(mapName);
            var expected = new MapChange(map).Compile();

            var ns = PacketTestUtilities.CreateTestNetState();
            ns.SendMapChange(map);

            var result = ns.SendPipe.Reader.TryRead();
            AssertThat.Equal(result.Buffer[0].AsSpan(0), expected);
        }
    }
}
