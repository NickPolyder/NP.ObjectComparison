# Change log

### 0.9.9.1

Performance fixes.

On `AnalyzerBuilder<>` 
- Cache properties and generated analyzers
- Pass the local options down to the analyzer builders.

On `Builder Strategies`
- Test Depth for nullability along with options.

On `AnalyzerSettings`
- The DefaultSettings generator now passes a cached version of the settings.

On `ComparisonTracker<>`
- Added one more constructor to accept external `AnalyzerSettings`


Added a `Benchmarks` project to help with performance improvements overtime.

### 0.9.9.0

Rename the `Skip` related functionality to `Ignore` in order to follow the general naming conventions.

On `ComparisonTracker<>`
- The history item that is reverted uses a copy of the item.
- Simplify constructor.