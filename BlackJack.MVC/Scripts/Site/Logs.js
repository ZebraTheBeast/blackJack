$(document).ready(function () {

	$("#grid").kendoGrid({
		dataSource: {
			transport: {
				read: {
					url: "/api/values/GetLogs",
					type: "GET"
				}
			},
			schema: {
				type: "json",
				model: {
					fields: {
						Id: { field: "Id", type: "number" },
						Time: { field: "Time", type: "string" },
						Message: { field: "Message", type: "string" }
					}
				}
			},
			pageSize: 14
		},
		pageable: true,
		columns: [
			{ field: "Id", width: "10%" },
			{ field: "Message" },
			{ field: "Time", width: "30%" }

		]
	});

});