// 反查数据grid
var peggingFinancialDataGrid = Ext.create('Financial.BaseGrid', {
    id: "PeggingFinancialDataGrid",
    url: "/FinancialData/PeggingFinancialData",
    autoLoad: false,
    pageSize: 100,
    showBbar: true,
    fields: ["AccountName", "ShiYeBu", "PianQu", "XingZhi", "XiangMu", "DetailData"],
    columns: [
        { text: '科目', dataIndex: 'AccountName', width:120},
        { text: '事业部', dataIndex: 'ShiYeBu', width: 120 },
        { text: '片区', dataIndex: 'PianQu', width: 120 },
        { text: '性质', dataIndex: 'XingZhi', width: 120 },
        { text: '项目', dataIndex: 'XiangMu', width: 120 },
        { text: '数据', dataIndex: 'DetailData', width: 120 }
    ]
});

// 反查数据window
var peggingFinancialDataWin = Ext.create("Ext.window.Window",
    {
        title: '反查数据',
        modal: true,
        constrainHeader: true,
        resizable: false,
        height: 600,
        width: 1000,
        layout: "fit",
        items: [peggingFinancialDataGrid],
        closeAction: 'hide' //close 关闭  hide  隐藏
    });

// 反查数据方法
function PeggingFinancialDataFunc(excelId, accountCode, qiJianTypeId, xiangMuItemId) {
    peggingFinancialDataWin.show();
    peggingFinancialDataGrid.store.loadWithParams({ ExcelRecordId: excelId, AccountCode: accountCode, QiJianTypeId: qiJianTypeId, XiangMuItemId: xiangMuItemId});
}