Ext.define('Financial.BaseStore', {
    extend: 'Ext.data.Store',
    constructor: function (config) {
        var me = this;
        config = Ext.applyIf(config, { url: "", fields: [], pageSize:10, autoLoad:false });
        me.callParent([Ext.apply({
            baseParams: { excelId: 1 },
            fields: config.fields,
            pageSize: config.pageSize,  //页容量5条数据
            //是否在服务端排序 （true的话，在客户端就不能排序）
            remoteSort: false,
            remoteFilter: true,
            proxy: {
                type: 'ajax',
                url: config.url,
                actionMethods: {
                    create: 'POST',
                    read: 'POST'
                },
                reader: {   //这里的reader为数据存储组织的地方，下面的配置是为json格式的数据，例如：[{"total":50,"rows":[{"a":"3","b":"4"}]}]
                    type: 'json', //返回数据类型为json格式
                    root: 'rows',  //数据
                    totalProperty: 'total' //数据总条数
                }
            },
            autoLoad: config.autoLoad  //即时加载数据
        }, config)]);
    },
    loadWithParams: function(config) {
        var me = this;
        me.baseParams = Ext.apply(me.baseParams, config || {});
        Ext.apply(me.proxy.extraParams, config || {});
        me.loadPage(1);
    }
});　　