# Results &middot; [![PRs Welcome](https://img.shields.io/badge/PRs-welcome-brightgreen.svg)](https://help.github.com/articles/creating-a-pull-request/) [![GitHub license](https://img.shields.io/badge/license-MIT-blue.svg)](./LICENSE) 

Carvana Results provides a standarized approach to Service-To-Service messaging, with an emphasis on explicit communication of Error details.

----

## Features

1. `Result` Data Structure
2. `IExternal` Interface
3. Functional Extension Methods for workflow declarations

## Installation

Carvana Results is available on NuGet.org. 

```Install-Package Carvana.Results```

```dotnet add package Carvana.Results```

## Contributing

The main purpose of this repository is to share Carvana Results with the .NET ecosystem and to continue to evolve Carvana Results, making it easier to use and more feature-rich. 

### License

Mediator is [MIT licensed](./LICENSE).

----

Carvana.Results supports both Functional as well as Procedural execution.

## Functional Usage Samples/Examples

---

**Return Value Or Default**

```
public async Task<Salad> GetSaladOrDefault(string saladId) => await _db.GetSalad(saladId).OrDefault(() => new Salad());
```

**Use Content on Success**

```
public Result<Salad> MakePotatoSalad()
  => _db.GetPotatoes()
    .Then(potatoes => new Salad(potatoes));
```

**Execute On Error Action**

```
public async Task<Result<Salad>> MakePotatoSalad()
  => await _db.GetPotatoes()
    .Then(potatoes => new Salad(potatoes))
    .OnFailure(x => Log.Error($"Unable to make potato salad. Error: {x}"));
```

**Verify Content on Success**

```
public Result<Salad> MakePotatoSalad()
  => _db.GetPotatoes()
    .Verify(potatoes => potatoes.Any() ? potatoes : potatoes.AsTypedError<Salad>("No Potatoes Found"))
    .OnFailure(x => Log.Error(x.ErrorMessage));
```

**Override Error Message**

```
public async Task<Result<Salad>> MakePotatoSalad()
  => await _db.GetPotatoes()
    .IfFailedWithErrorMessage(x => $"StatusCode: {x.StatusCode} There was an error when attempting to make potato salad.")
    .Then(potatoes => new Salad(potatoes));
```

**Prepend Error Message**

```
public async Task<Result<Salad>> MakePotatoSalad()
  => await _db.GetPotatoes()
    .OnFailurePrependMessage(x => "Unable to make potato salad. Error: ")
    .Then(potatoes => new Salad(potatoes));
```

**Use Content Asynchronously On Success**

```
public async Task<Result<Salad>> MakePotatoSalad()
  => await _db.GetPotatoesAsync()
    .Then(potatoes => MakeSaladAsync(potatoes));
```

**Perform Action on Success Without Changing Result**

```
public async Task<Result<Salad>> GetSaladWithLogging(string saladId)
  => await _db.GetSaladAsync(saladId)
    .OnSuccess(salad => Log.Info($"Successfully retrieved salad with id {saladId}."));
```

**Basic Validation Before Executing and Using Result Content on Success**

```
public async Task<Result<Salad>> MakePotatoSalad()
{
  var validationResp = await EnsureThatCustomerOrderedPotatoSalad();
  if (validationResp.Failed())
    return validationResp.AsTypedError<Salad>(ResultStatus.InvalidRequest, "Customer did not order potato salad");

  return await _db.GetPotatoes()
    .Then(potatoes => new Salad(potatoes));
}
```

**Complex Result Chaining**

```
public async Task<Salad> MakePotatoSalad()
  => await _db.GetPotatoes()
    .Then(potatoes => potatoes.Count > 1 ? potatoes : Result<Ingredient>.Error(ResultStatus.ProcessingError, "Cannot make potato salad with just a single potato"))
    .IfErroredThen(potatoes => LogFailedPotatoesRetrieval(potatoes))
    .Then(potatoes => new Salad(potatoes))
    .OnSuccess(salad => PostPictureOfPotatoSaladToTwitter(salad));
```

**Concatinating Result Contents**

```
public async Task<PotatoSalad> CombinePotatoWithSalad(string potatoId, string saladId)
  => await _db.GetPotato(potatoId)
    .ThenConcatWith(potato => _db.GetSalad(saladId))
    .Then(potatoAndSalad => new PotatoSalad(potatoAndSalad.Item1, potatoAndSalad.Item2));
```

**Concatinating Result Contents with Explicit Typing**

```
public async Task<PotatoSalad> CombinePotatoWithSalad(string potatoId, string saladId)
  => await _db.GetPotato(potatoId)
    .ThenConcatWith(potato => _db.GetSalad(saladId))
    .Then(((Potato potato, Salad Salad) potatoAndSalad) => new PotatoSalad(potatoAndSalad.Potato, potatoAndSalad.Salad));
```


## Procedural Usage Samples/Examples

----

**Return Value Or Default**

```
public Salad MakePotatoSalad()
{
  var potatoesResult = _db.GetPotatoes();
  if (potatoesResult.Failed())
    return new Salad();

  var potatoes = resp.Content;
  return new Salad(potatoes);
}
```

**Basic Success**

```
public async Task<Result<Salad>> MakePotatoSalad()
{
  var resp = await _db.getPotatoes();
  if (resp.Failed())
    return resp;

  var potatoes = resp.Content;
  return new Salad(potatoes);
}
```

**On Success Action**

```
public async Task<Result<Salad>> MakePotatoSalad()
{
  var potatoesResult = await _db.GetPotatoes();
  if (potatoesResult.Succeeded())
  {
    Log.Info($"Successfully retrieved potatoes."));
    return new Salad(potatoesResult.Content);
  }
  return potatoesResult.AsTypedError<Salad>();
}
```

**Execute On Error Action**

```
public async Task<Result<Salad>> MakePotatoSalad()
{
  var potatoesResult = await _db.GetPotatoes();
  if (potatoesResult.Failed())
  {
    Log.Error($"Unable to make potato salad. Error: {potatoesResult.ErrorMessage}");
    return await potatoesResult.AsTypedError<Salad>();
  }
  return new Salad(potatoesResult.Content);
}
```

**Verify Content on Success**

```
public async Task<Result<Salad>> MakePotatoSalad()
{
  var potatoesResult = await _db.GetPotatoes();
  if (potatoesResult.Failed())
    return potatoesResult.AsTypedError<Salad>();

  if (!potatoesResult.Content.Any())
    return potatoesResult.AsTypedError<Salad>("No Potatoes Found.");

  Log.Info($"Successfully retrieved potatoes."));
  return new Salad(potatoesResult.Content);
}
```

**Prepend Error Message**

```
public async Task<Result<Salad>> MakePotatoSalad()
{
  var potatoesResult = await _db.GetPotatoes();
  if (potatoesResult.Failed())
    return new Result<Salad>(potatoesResult.StatusCode, $"StatusCode: {x.StatusCode} There was an error when attempting to make potato salad.");
  return new Salad(potatoesResult.Content);
}
```

**Override Error Message**

```
public async Task<Result<Salad>> MakePotatoSalad()
{
  var potatoesResult = await _db.GetPotatoes();
  if (potatoesResult.Failed())
    return await potatoesResult.AsTypedError<Salad>("Unable to make potato salad. Error: ");
  return new Salad(potatoesResult.Content);
}
```

**Basic Validation With Call That Maps to Target Return Type**

```
public async Task<Result<Salad>> MakePotatoSalad()
{
  var validationResp = await EnsureThatCustomerOrderedPotatoSalad();
  if (validationResp.Failed())
    return validationResp.AsTypedError<Salad>(ResultStatus.InvalidRequest, "Customer did not order potato salad");

  var potatoesResult = await _db.GetPotatoes()
  if (potatoesResult.Failed())
    return potatoesResult.AsTypedError<Salad>("Unable to retrieve potatoes from db.");
  return new Salad(potatoesResult.Content);
}
```

**Complex Result Chaining**

```
public async Task<Salad> MakePotatoSalad()
{
  var potatoesResult = await _db.GetPotatoes();
  if (potatosResult.Failed())
  {
    LogFailedPotatoesRetrieval(potatoes);
    return potatoesResult.AsTypedError<Salad>();
  }

  if (potatoes.Count <= 1)
  {
    LogFailedPotatoesRetrieval(potatoes);
    return Result<Ingredient>.Error(ResultStatus.ProcessingError, "Cannot make potato salad with just a single potato"))
  }

  var salad = new Salad(potatoes);
  await PostPictureOfPotatoSaladToTwitterAsync(salad);
  return salad;
}
```

**Concatinating Result Contents**

```
public async Task<PotatoSalad> CombinePotatoWithSalad(string potatoId, string saladId)
{
  Result<Potato> potatoResult = await _db.GetPotato(potatoId);
  if (potatoResult.Failed())
    return potatoResult.AsTypedError<PotatoSalad>("Failed to acquire potato from db.");

  Salad salad = await _db.GetSalad(saladId);
  if (salad.Failed())
    return salad.AsTypedError<PotatoSalad>("Failed to acquire salad from db.");

  Potato potato = potatoResult.Content;
  Salad salad = saladResult.Content;

  return new PotatoSalad(potato, salad);
}
```

