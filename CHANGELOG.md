# Change Log

All notable changes to this project will be documented in this file. See [versionize](https://github.com/saintedlama/versionize) for commit guidelines.

<a name="1.0.0-alpha.7"></a>
## [1.0.0-alpha.7](https://www.github.com/Kantaiko/Controllers/releases/tag/v1.0.0-alpha.7) (2023-2-26)

### Bug Fixes

* Don't assign NotMatched error if another error is already assigned ([0a02a09](https://www.github.com/Kantaiko/Controllers/commit/0a02a096fd85df5a0bec62932a023ef7232f3a70))
* Provide match properties from matching result even on error ([edba5c6](https://www.github.com/Kantaiko/Controllers/commit/edba5c6675a2467ec03a879cfa3a0237833bde7e))
* Set endpoint on matching error ([47df5b3](https://www.github.com/Kantaiko/Controllers/commit/47df5b3832225057b66259bd92b6e0fd9bbcabb8))

<a name="1.0.0-alpha.6"></a>
## [1.0.0-alpha.6](https://www.github.com/Kantaiko/Controllers/releases/tag/v1.0.0-alpha.6) (2023-2-25)

### Features

* Add endpoint to ControllerExecutionResult ([5cefd6f](https://www.github.com/Kantaiko/Controllers/commit/5cefd6fc83e8e9fe2fb8c0be3fc9586f2391292f))
* Add the ability to not return an endpoint matching error immediately ([797610e](https://www.github.com/Kantaiko/Controllers/commit/797610e4ef10b6cc21bc10a4f12d1c6497505c0e))

### Bug Fixes

* Add missing symbols to the public api ([ad6634d](https://www.github.com/Kantaiko/Controllers/commit/ad6634d5f59c83d080ea9948a9bef45fd88c1319))
* Enable documentation generation ([8ff4716](https://www.github.com/Kantaiko/Controllers/commit/8ff4716bc6d4d9bbaca98aa0ee2adc23dee5a38c))
* Make Handlers and Transformers type the IList in ControllerExecutorBuilder ([1f22e6c](https://www.github.com/Kantaiko/Controllers/commit/1f22e6cbca163cbf01e9c722c131c1a899dbeffc))

<a name="1.0.0-alpha.5"></a>
## [1.0.0-alpha.5](https://www.github.com/Kantaiko/Controllers/releases/tag/v1.0.0-alpha.5) (2023-2-23)

### Features

* Add ability to not inherit endpoint matchers ([609c846](https://www.github.com/Kantaiko/Controllers/commit/609c846a2fcb4f65cd40cc76a2e2775f5f661d43))
* Add MatchProperties to parameter conversion errors ([8719429](https://www.github.com/Kantaiko/Controllers/commit/8719429e07ce3f38c7b526840f674a0acb00aadd))

<a name="1.0.0-alpha.4"></a>
## [1.0.0-alpha.4](https://www.github.com/Kantaiko/Controllers/releases/tag/v1.0.0-alpha.4) (2023-2-10)

### Bug Fixes

* Add properties to typed EndpointMatchingContext ([0e1e92e](https://www.github.com/Kantaiko/Controllers/commit/0e1e92e6d15c2577917202463dd0209fcdd0c750))

<a name="1.0.0-alpha.3"></a>
## [1.0.0-alpha.3](https://www.github.com/Kantaiko/Controllers/releases/tag/v1.0.0-alpha.3) (2023-2-10)

### Features

* Add sync text converters ([d43f0da](https://www.github.com/Kantaiko/Controllers/commit/d43f0da6bb8acb01b05967af8ad480f803e1b5dc))
* Expose execution context properties in EndpointMatchingContext ([e5c0660](https://www.github.com/Kantaiko/Controllers/commit/e5c066064f78a974b4bda5efbb2dd78987fde984))

<a name="1.0.0-alpha.2"></a>
## [1.0.0-alpha.2](https://www.github.com/Kantaiko/Controllers/releases/tag/v1.0.0-alpha.2) (2023-2-8)

### Features

* Add ability not to await result in EndpointInvocationHandler ([274477b](https://www.github.com/Kantaiko/Controllers/commit/274477bb0c5fd52b2a12a3458c11352ecf4bdff1))
* Expose the IntrospectionInfo in ControllerExecutor ([88c6fcb](https://www.github.com/Kantaiko/Controllers/commit/88c6fcb8728d14b0d26d21ec42ca70c93342f92d))
* Simplify text conversion system ([dbaa93f](https://www.github.com/Kantaiko/Controllers/commit/dbaa93ffd29c5f636a658d5a76049e0b3b2c349a))

<a name="1.0.0-alpha.1"></a>
## [1.0.0-alpha.1](https://www.github.com/Kantaiko/Controllers/releases/tag/v1.0.0-alpha.1) (2023-2-3)

### Features

* Yet another epic redesign ([1f2f850](https://www.github.com/Kantaiko/Controllers/commit/1f2f8503b35b7bf634704c8d355786f7175aa6d1))

<a name="1.0.0-alpha.0"></a>
## [1.0.0-alpha.0](https://www.github.com/Kantaiko/Controllers/releases/tag/v1.0.0-alpha.0) (2022-8-12)

### Features

* Remove dependency on Kantaiko.Routing ([06edfc5](https://www.github.com/Kantaiko/Controllers/commit/06edfc51d41bbfba81399206e9932e72f38a3607))

<a name="0.8.2"></a>
## [0.8.2](https://www.github.com/Kantaiko/Controllers/releases/tag/v0.8.2) (2022-7-17)

### Bug Fixes

* Make AwaitAsyncResultHandler correctly handle async method results ([99c0f7b](https://www.github.com/Kantaiko/Controllers/commit/99c0f7ba732c93e6c4c758586796f2ee3728c8c7))

<a name="0.8.1"></a>
## [0.8.1](https://www.github.com/Kantaiko/Controllers/releases/tag/v0.8.1) (2022-7-16)

### Bug Fixes

* Update routing infrastructure ([03be137](https://www.github.com/Kantaiko/Controllers/commit/03be13781c862622b7bbbd9fa59ef0d77e914562))

<a name="0.8.0"></a>
## [0.8.0](https://www.github.com/Kantaiko/Controllers/releases/tag/v0.8.0) (2022-7-16)

### Features

* Update routing infrastructure ([27e581f](https://www.github.com/Kantaiko/Controllers/commit/27e581f508b4767750107d965c0a026ca0389d8d))

<a name="0.7.3"></a>
## [0.7.3](https://www.github.com/Kantaiko/Controllers/releases/tag/v0.7.3) (2022-6-28)

### Bug Fixes

* Disable parameter deconstruction with attribute-specified converters ([124b88c](https://www.github.com/Kantaiko/Controllers/commit/124b88c29a0ba17dc4ff73596d5dce35244102da))
* Fix nullability in ParameterAttribute ([61052f0](https://www.github.com/Kantaiko/Controllers/commit/61052f07b093736fc816d569cc430c5b53250ab2))

<a name="0.7.2"></a>
## [0.7.2](https://www.github.com/Kantaiko/Controllers/releases/tag/v0.7.2) (2022-2-5)

### Bug Fixes

* Transform children in CustomizationIntrospectionInfoTransformer ([e9b6e81](https://www.github.com/Kantaiko/Controllers/commit/e9b6e81dcb484bcd0d3324894bda380115c1b1dc))
* Use all available property providers in PropertyProviderIntrospectionInfoTransformer ([c8af6b8](https://www.github.com/Kantaiko/Controllers/commit/c8af6b818fdabe5f6cc5310b59ef5abf8428d2df))

<a name="0.7.1"></a>
## [0.7.1](https://www.github.com/Kantaiko/Controllers/releases/tag/v0.7.1) (2022-2-5)

### Bug Fixes

* Transform children in CustomizationIntrospectionInfoTransformer ([7dd62fc](https://www.github.com/Kantaiko/Controllers/commit/7dd62fc8e6c31fbc941933cdfd4c89255a52b79f))

<a name="0.7.0"></a>
## [0.7.0](https://www.github.com/Kantaiko/Controllers/releases/tag/v0.7.0) (2022-1-8)

### Features

* Add VisibilityParameterProperties ([fd7756a](https://www.github.com/Kantaiko/Controllers/commit/fd7756a72a8d0c7fec590e049a6a911c4971da04))

<a name="0.6.1"></a>
## [0.6.1](https://www.github.com/Kantaiko/Controllers/releases/tag/v0.6.1) (2022-1-7)

### Bug Fixes

* Always assign context.Result in AwaitAsyncResultHandler ([ef85fe6](https://www.github.com/Kantaiko/Controllers/commit/ef85fe6a0d992b31644cc89b4b41633ddb5f5e41))

<a name="0.6.0"></a>
## [0.6.0](https://www.github.com/Kantaiko/Controllers/releases/tag/v0.6.0) (2022-1-5)

### Features

* Redesign the entire controller infrastructure ([81fb9f5](https://www.github.com/Kantaiko/Controllers/commit/81fb9f50e3036efc7a939564834e753bd97c746c))

<a name="0.5.0"></a>
## [0.5.0](https://www.github.com/Kantaiko/Controllers/releases/tag/v0.5.0) (2021-11-12)

### Features

* Treat nullable reference types as optional parameter types ([79f83ca](https://www.github.com/Kantaiko/Controllers/commit/79f83ca7fa7c764cc1ff31b66fecd778e467903f))

<a name="0.4.0"></a>
## [0.4.0](https://www.github.com/Kantaiko/Controllers/releases/tag/v0.4.0) (2021-8-15)

### Features

* Add IsHidden parameter design property ([7ca4b86](https://www.github.com/Kantaiko/Controllers/commit/7ca4b86bf920dfa6a81c7e5de6acf31d64fb20de))
* Improve design property system ([db6fb9d](https://www.github.com/Kantaiko/Controllers/commit/db6fb9d3865e955a8178d03fe18241616562a6db))

<a name="0.3.0"></a>
## [0.3.0](https://www.github.com/Kantaiko/Controllers/releases/tag/v0.3.0) (2021-8-5)

### Features

* Expose ControllerInstance and ProcessingResult in middleware execution context ([309b3bc](https://www.github.com/Kantaiko/Controllers/commit/309b3bcfd7295173631cbc88003f0b39f479c133))

<a name="0.2.1"></a>
## [0.2.1](https://www.github.com/Kantaiko/Controllers/releases/tag/v0.2.1) (2021-8-3)

### Bug Fixes

* Allow multiple request handlers with different request types ([f2c91af](https://www.github.com/Kantaiko/Controllers/commit/f2c91af94664b398fd795d821aee838d7c2e1e08))

<a name="0.2.0"></a>
## [0.2.0](https://www.github.com/Kantaiko/Controllers/releases/tag/v0.2.0) (2021-8-2)

### Features

* Add parameter default value resolvers ([47d535e](https://www.github.com/Kantaiko/Controllers/commit/47d535e62f13e78f556d8a930ffbcb674e71b8c8))

<a name="0.1.4"></a>
## [0.1.4](https://www.github.com/Kantaiko/Controllers/releases/tag/v0.1.4) (2021-7-31)

### Bug Fixes

* Prevent deconstruction of types with custom converter ([7090b81](https://www.github.com/Kantaiko/Controllers/commit/7090b81efc1fc49f9086ff9ce055473afb2cd6e2))

<a name="0.1.3"></a>
## [0.1.3](https://www.github.com/Kantaiko/Controllers/releases/tag/v0.1.3) (2021-7-29)

### Bug Fixes

* Escape PatternTextMatcher pattern ([a34491c](https://www.github.com/Kantaiko/Controllers/commit/a34491c0aef9510a4a811a7c54be3b7c52e74920))
* Fix FromServices attribute ([ba0244d](https://www.github.com/Kantaiko/Controllers/commit/ba0244d3ce17d72456e37bfd85e78bf873ab834a))
* Unwrap TargetInvocationException ([bd438a0](https://www.github.com/Kantaiko/Controllers/commit/bd438a0bbd7d5d8b02bff502aca183c5b5dd7305))

<a name="0.1.2"></a>
## [0.1.2](https://www.github.com/Kantaiko/Controllers/releases/tag/v0.1.2) (2021-7-28)

### Bug Fixes

* Update PatternTextMatcher pattern ([4033aaa](https://www.github.com/Kantaiko/Controllers/commit/4033aaaa855754917db63e027ed0da0de09e8aab))

<a name="0.1.1"></a>
## [0.1.1](https://www.github.com/Kantaiko/Controllers/releases/tag/v0.1.1) (2021-7-28)

### Bug Fixes

* Add ability to specify regex options in RegexTextMatcher ([fd6c33a](https://www.github.com/Kantaiko/Controllers/commit/fd6c33a6807314eabbdc8ab8510bd3cd5b92f9ae))
* Make MiddlewareCollection class public ([3b3438c](https://www.github.com/Kantaiko/Controllers/commit/3b3438c8ceffa7d26c9d3129cceaa953ca933be2))

<a name="0.1.0"></a>
## [0.1.0](https://www.github.com/Kantaiko/Controllers/releases/tag/v0.1.0) (2021-7-26)

