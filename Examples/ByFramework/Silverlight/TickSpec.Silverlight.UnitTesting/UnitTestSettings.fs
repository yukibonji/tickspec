﻿namespace TickSpec

open Microsoft.Silverlight.Testing
open Microsoft.Silverlight.Testing.Harness
open Microsoft.Silverlight.Testing.UnitTesting.Metadata

[<RequireQualifiedAccess>]
module UnitTestSettings =
    let Make features =
        let settings = UnitTestSettings()
        let harness = UnitTestHarness()
        settings.TestHarness <- harness
        harness.Settings <- settings
        harness.Initialize()
        harness.TestRunStarting.Subscribe(fun ex ->
            let provider = TestProvider()
            let filter = TagTestRunFilter(settings, harness, settings.TagExpression)
            for feature in features do
                provider.RegisterFeature feature
                let provider = provider :> IUnitTestProvider
                let assembly = provider.GetUnitTestAssembly(harness,feature.Assembly)
                harness.EnqueueTestAssembly(assembly,filter)
        ) |> ignore
        settings