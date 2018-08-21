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
						Id: {
							field: "Id",
							type: "number"
						},
						Time: {
							field: "Time",
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
				field: "Id",
				width: "10%",
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
				width: "30%",
				format: "{0:dd.MM.yyyy HH:mm:ss}"
			}
		],
		sortable: true
	});

});