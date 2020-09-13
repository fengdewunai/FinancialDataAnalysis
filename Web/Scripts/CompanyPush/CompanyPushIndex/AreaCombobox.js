Ext.define('areaComboboxModel', {
    extend: 'Ext.data.Model',
    fields: [{
        name: 'key',
        type: 'string'
    }, {
        name: 'value',
        type: 'string'
    }]
});

var areaComboboxStore = Ext.create('Ext.data.Store', {
    model: 'areaComboboxModel',
    proxy: {
        type: 'ajax',
        url: '/CompanyPush/GetAllArea',
        reader: {
            type: 'json',
            root: 'data'
        }
    },
    autoLoad: true
});
Ext.define('CompanyPush.AreaComboBox', {
    extend: 'Ext.form.field.ComboBox',
    xtype: 'areacombobox',
    valueField: "key",
    displayField: 'value',
    store: areaComboboxStore,
    queryMode: "local",
    forceSelection: true,//只能选，不能输入文本
    typeAhead: true//如果匹配到已知的值将填充和自动选择键入的文本其余部分
});