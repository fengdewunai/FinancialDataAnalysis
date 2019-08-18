Ext.define('Financial.TreeStore', {
    extend: 'Ext.data.TreeStore',
    constructor: function (config) {
        var me = this;
        config = Ext.applyIf(config, { url: "", rootName: "", extraParams: {} });
        me.callParent([Ext.apply({
            fields: ["id", "text", "leaf", "expanded"],
            proxy: {
                type: 'ajax',
                url: config.url,
                extraParams: config.extraParams,
                actionMethods: {
                    create: 'POST',
                    read: 'POST'
                },
                reader: {
                    type: 'json',
                    root: 'root'     //根节点参数名
                }
            },
            nodeParam: 'id',   //节点参数名，当节点展开时向服务端传送id:
            
            root: {
                id: '0',
                text: this.rootName,   //根节点显示的名称
                expanded: true     //是否默认展开
            }
        }, config)]);
    }
});　　

Ext.define('Financial.BaseTree', {
    extend: 'Ext.tree.Panel',
    alias: 'BaseTree',
    store:null,
    style: "margin-left:2px",
    displayField: 'text',     //此处为传输数据的{'text':'集团','leaf':'false','id':'1'}
    useArrows: true,　　　　　　//展开样式，默认为+
    multiSelect: false,　　　　//多选模式
    rootVisible: true,　　　　//是否显示根节点
    autoScroll: true,　　　　　　//是否自动添加滚动条
    constructor: function (config) {
        var me = this;
        config = Ext.applyIf(config, { url: "", rootName: "", extraParams: {} });
        var treeStore = Ext.create('Financial.TreeStore', {
            url: config.url,
            extraParams: config.extraParams,
            rootName: config.rootName
        });
        if (config.rootName == "") {
            me.rootVisible = false;
        }
        me.store = treeStore;
        me.callParent([Ext.apply({}, config)]);
    },
    getSelectedNodes: function() {
        var me = this;
        var result = [];
        var selectedNodes = me.getChecked();
        Ext.each(selectedNodes, function(node) {
            result.push(node.raw.id);
        }) 
        return result;
    }
});
