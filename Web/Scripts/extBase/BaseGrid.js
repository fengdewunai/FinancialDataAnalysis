Ext.define('Financial.BaseGrid', {
    extend: 'Ext.grid.Panel',
    title: "",
    style: "margin-left:3px",
    selModel: { selType: 'checkboxmodel' }, //选择框
    columns:[],
    constructor: function (config) {
        var me = this;
        config = Ext.applyIf(config, { url: "", fields: [], pageSize: 100, columns: [], autoLoad:false, showBbar:false });
        var dataStore = Ext.create('Financial.BaseStore', {
            url: config.url,
            fields: config.fields,
            pageSize: config.pageSize,
            autoLoad: config.autoLoad
        });
        me.store = dataStore;
        me.columns = config.columns;
        delete config.autoLoad;
        if (config.showBbar) {
            me.bbar = [{
                xtype: 'pagingtoolbar',
                store: dataStore,
                displayMsg: '显示 {0} - {1} 条，共计 {2} 条',
                emptyMsg: "没有数据",
                beforePageText: "当前页",
                afterPageText: "共{0}页",
                displayInfo: true
            }];
        }
        me.callParent([Ext.apply({}, config)]);
    }
});