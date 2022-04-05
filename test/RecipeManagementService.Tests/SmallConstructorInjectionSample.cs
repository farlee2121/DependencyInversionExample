using System;
using Xunit;

namespace SmallConstructorInjectionSample;

public class SmallConstructorInjectionSampleTests
{
    [Fact]
    public void SeeItRun(){
        // Dependencies composed only in client. Note TopLevel does not directly know about 2nd+ degree dendencies (i.e. Lowest service or lowest service config)
        // and both Mid1 and Mid1 don't know about any dependencies of Lowest (in this case just config)
        var sut = new TopLevel(
            new Mid1(
                new Mid1.Config("I'm configured"), 
                // note multiple uses of lowest service. Each can be configured differently
                new LowestService(new LowestService.Config("something", 1))
            ),
            new Mid2(
                new LowestService(new LowestService.Config("something else", 2))
            )
        );

        var testString = sut.TestMe();
    }
}


public class TopLevel
{
    private readonly Mid1 dep1;
    private readonly Mid2 dep2;
    public TopLevel(Mid1 dep1, Mid2 dep2)
    {
        this.dep1 = dep1;
        this.dep2 = dep2;
    }

    public string TestMe()
    {
        return $"TopLevel with dep1:|{dep1.TestMe()}| and dep2: |{dep2.TestMe()}|";
    }
}

public class Mid1
{
    public record Config(string message);

    private readonly Config config;
    private readonly LowestService lowest;
    public Mid1(Config config, LowestService lowest)
    {
        this.config = config;
        this.lowest = lowest;
    }

    public string TestMe()
    {
        return $"(Mid1 with {config.message} and |{lowest.TestMe()}|)";
    }
}

public class Mid2
{
    private readonly LowestService lowest;
    public Mid2(LowestService lowest)
    {
        this.lowest = lowest;
    }

    public string TestMe()
    {
        return $"(Mid2 with |{lowest.TestMe()}|)";
    }
}

public class LowestService
{
    public record Config(string message, int otherConfig);


    private readonly Config config;
    public LowestService(Config config)
    {
        this.config = config;
    }

    public string TestMe()
    {
        return $"(Lowest: {config.message}, {config.otherConfig})";
    }
}