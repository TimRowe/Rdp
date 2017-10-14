var dataTables={};
dataTables.i18n={};
dataTables.i18n.getLang = function(lang)
{
	switch (lang){
		case 'zh-CN':
			return {
				"sProcessing":   "处理中...",
				"sLengthMenu":   "显示 _MENU_ 项结果",
				"sZeroRecords":  "没有匹配结果",
				"sInfo":         "显示第 _START_ 至 _END_ 项结果，共 _TOTAL_ 项",
				"sInfoEmpty":    "显示第 0 至 0 项结果，共 0 项",
				"sInfoFiltered": "(由 _MAX_ 项结果过滤)",
				"sInfoPostFix":  "",
				"sSearch":       "搜索:",
				"sUrl":          "",
				"sEmptyTable":     "表中数据为空",
				"sLoadingRecords": "载入中...",
				"sInfoThousands":  ",",
				"oPaginate": {
					"sFirst":    "首页",
					"sPrevious": "上页",
					"sNext":     "下页",
					"sLast":     "末页"
				},
				"oAria": {
					"sSortAscending":  ": 以升序排列此列",
					"sSortDescending": ": 以降序排列此列"
				}
			};
		case 'zh-TW':
			return {
				"processing": "處理中...",
				"loadingRecords": "載入中...",
				"lengthMenu": "顯示 _MENU_ 項結果",
				"zeroRecords": "沒有符合的結果",
				"info": "顯示第 _START_ 至 _END_ 項結果，共 _TOTAL_ 項",
				"infoEmpty": "顯示第 0 至 0 項結果，共 0 項",
				"infoFiltered": "(從 _MAX_ 項結果中過濾)",
				"infoPostFix": "",
				"search": "搜尋:",
				"paginate": {
					"first": "第一頁",
					"previous": "上一頁",
					"next": "下一頁",
					"last": "最後一頁"
				},
				"aria": {
					"sortAscending": ": 升冪排列",
					"sortDescending": ": 降冪排列"
				}
			};
		default:
			return {
				"sEmptyTable": "No data available in table",
				"sInfo": "Showing _START_ to _END_ of _TOTAL_ entries",
				"sInfoEmpty": "Showing 0 to 0 of 0 entries",
				"sInfoFiltered": "(filtered from _MAX_ total entries)",
				"sInfoPostFix": "",
				"sInfoThousands": ",",
				"sLengthMenu": "Show _MENU_ entries",
				"sLoadingRecords": "Loading...",
				"sProcessing": "Processing...",
				"sSearch": "Search:",
				"sZeroRecords": "No matching records found",
				"oPaginate": {
					"sFirst": "First",
					"sLast": "Last",
					"sNext": "Next",
					"sPrevious": "Previous"
				},
				"oAria": {
					"sSortAscending": ": activate to sort column ascending",
					"sSortDescending": ": activate to sort column descending"
				}
			};
	}
}

