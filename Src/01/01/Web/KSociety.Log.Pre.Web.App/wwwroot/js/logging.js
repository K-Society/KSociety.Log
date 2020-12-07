"use strict";

var app = angular.module('eventLogApp', ['ui.grid'])
    .controller('EventLogCtrl', EventLogCtrl);

var rowTemplate = function () {
    return '<div ng-class="{WhiteRow:row.entity.level===0, GreyRow:row.entity.level===1, GreenRow:row.entity.level===2, OrangeRow:row.entity.level===3, RedRow:row.entity.level===4,  PurpleRow:row.entity.level===5}" ' +
        '<div ng-repeat="col in colContainer.renderedColumns track by col.colDef.name"  class="ui-grid-cell" ui-grid-cell></div></div>';
}

EventLogCtrl.$inject = ['$scope', 'uiGridConstants'];

function EventLogCtrl($scope, uiGridConstants) {
    $scope.eventLogData = [];
    $scope.gridOptions = {
        enableSorting: true,
        enableFiltering: true,
        enableColumnResizing: true,

        columnDefs: [
            { field: 'timeStamp', name: 'Date', cellFilter: 'date:"yyyy-MM-dd HH:mm:ss.sss"', sort: { direction: uiGridConstants.DESC }, width: 190 },
            //{ field: 'level', name: 'Level', width: 75 },
            //{ field: 'loggerName', name: 'Logger', width: 150 },
            { field: 'message', name: 'Message', minWidth: 500, maxWidth: 1000 }
        ],
        rowTemplate: rowTemplate(),
        data: 'eventLogData'
        //afterSelectionChange: function (rowItem, event) { alert(rowItem.entity.Message); }
    };

    $scope.changeData = function (logEvent) {
        $scope.eventLogData.push(logEvent);
    };

    $scope.deleteRows = function () {
        if ($scope.eventLogData.length >= 150) {
            $scope.eventLogData.splice(0, 1);
            //$scope.myData.removeRow(0,1);
        }
    }
}

var connection = new signalR.HubConnectionBuilder().withUrl("/loggingHub").build();

connection.on("ReceiveLog",
    function (logEvent) {
        var scope = angular.element($("#scope")).scope();
        scope.changeData(logEvent);
        scope.deleteRows();
        scope.$apply();
    });

connection.logging = true;

connection.start().then(function () {
    //alert("Connection ok! ");
}).catch(function (err) {
    alert("Could not Connect! " + err.toString());
});