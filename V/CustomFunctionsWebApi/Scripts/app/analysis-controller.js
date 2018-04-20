angular.module('CodeAnalysis', [
        'ngRoute'
    ])

    .config(['$routeProvider',
        function ($routeProvider) {
            $routeProvide.when('/functionsummary',
                {
                    templateUrl: 'p'
                })
        }
    ])

    .controller('CustomFunctionCtrl', function ($scope, $http) {
        $scope.message = "Custom Functions";
        $scope.currentDatabase = "Select a database..."

        $scope.databases = [];
        $scope.customFunctions = [];

        $scope.echo = function (data) {
            return data;
        };

        $scope.selectDatabase = function (dbInfo) {
            console.log(dbInfo);
            $scope.currentDatabase = dbInfo.DB;

            var url = "/api/FunctionSummaries?$filter=DB eq '"+dbInfo.DB+"'";
            $http.get(url)
                .success(function (data, status, headers, config) {
                    console.log(data);
                    $scope.currentDatabase = dbInfo.DB;
                    $scope.customFunctions = data.value;
                })
                .error(function (data, status, headers, config) {
                    $scope.currentDatabase = "Unable to access " + dbInfo.DB;
                    $scope.customFunctions = [];
                });
        }

        
        $http.get("/api/Databases")
            .success(function (data, status, headers, config) {
                console.log(data);
                $scope.databases = data.value;
                //console.log($scope.databases);
                $scope.message = 'Databases loaded'
            })
            .error(function (data, status, headers, config) {
                $scope.message = "Unable to load databases"
            });
        
    });