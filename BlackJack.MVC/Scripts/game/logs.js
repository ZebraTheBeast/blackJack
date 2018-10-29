$(document).ready(function () {

	$("#grid").kendoGrid({
		dataSource: {
			transport: {
				read: {
					url: "/api/logApi/GetLogs",
					type: "GET"
				}
			},
			schema: {
				type: "json",
				model: {
					fields: {
						PlayerId: {
							field: "PlayerId",
							type: "number"
						}, 
						GameId: {
							field: "GameId",
							type: "number"
						},
						Time: {
							field: "CreationDate",
							type: "date"
						},
						Message: {
							field: "Message",
							type: "string"
						}
					}
				}
			},
			pageSize: 14
		},
		pageable: true,
		filterable: true,
		columns: [
			{
				field: "GameId",
				width: "20%",
				filterable: {
					cell: {
						showOperators: false
					}
				}
			},
			{
				field: "PlayerId",
				width: "20%",
				filterable: {
					cell: {
						showOperators: false
					}
				}
			},
			{
				field: "Message",
				filterable: {
					cell: {
						operator: "contains",
						suggestionOperator: "contains"
					}
				}
			},
			{
				field: "Time",
				width: "0%",
				format: "{0:dd.MM.yyyy HH:mm:ss}"
			}
		],
		sortable: true
	});

});