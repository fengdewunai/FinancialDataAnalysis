/**************************************************************************************************
Copyright (C) Beijing Beida Software Engineering Development Co., Ltd. & Author
All rights reserved.
Author:Kenshin Cui
Date:3/26/2013 9:59:13 AM
Description:自定义数据展示扩展组件
Note:查询模板中不支持中文字段配置，请使用英文
     如果只是一般的简单提示，建议添加item元素的data-qtip属性，而不是配置该组件的tip选项
*************************************************************************************************/
Ext.define('BeidaSoft.app.View', {
    extend: 'BeidaSoft.view.View',
    alias: 'widget.bdaview',
    //id: 'appDataView', //由于DataViewTransition依赖于css，因此如果使用默认样式此id不能修改
    tpl: null, //视图模板，如果不设置将使用内置模板
    itemSelector: 'div.viewitem', //此属性必须设置否则在低版本IE中报错
    overItemCls: 'viewitem-hover', //此属性ext4.x做了变更不再叫overClass
    singleSelect: true,
    multiSelect: true,
    autoScroll: true,
    loadMask: { msg: '数据加载中...' },
    columnCount: 0, //要显示的列数，默认为0，所有列均显示
    url: 'url...', //此url只是sql文件处理路径，同BeidaSoft.grid.GridPanel意义不同并非一个请求
    beforeBaseParams: null,
    //sortable: false, // 是否支持排序,这里指的是列排序
    //sortConfig: { tableName: '', primaryKey: '', sortField: '', primaryKeyValues: '' }, //primaryKeyValues是组件内部参数不对外公开
    cacheData: false, //是否对数据进行缓存
    autoLoad: false, //是否自动加载
    tip: false, //启用提示,如果启用可以配置{tpl:''}
    debug: false, //是否开启调试模式,调试模式（并且不缓存）的情况下可以查看后台sql
    initComponent: function () {
        var me = this;
        var ld = this.autoLoad;
        this.autoLoad = false; //extjs内部如果接收到autoLoad参数为true会报错，这里转移解决
        this.addEvents('load', 'sort'); //加载结束、排序完成后执行的事件
        //校验必须配置的项
        if (this.tpl === null) {
            console.info("请配置tpl属性！");
            return;
        } else if (this.url === '') {
            console.info("请配置url属性！");
            return;
        }
        var params = { //添加内部参数
            gridId: this.id,
            sqlPath: this.url,
            columnCount: this.columnCount,
            cacheData: this.cacheData,
            debug: this.debug
        };
        for (var p in this.beforeBaseParams) {
            params[p] = this.beforeBaseParams[p];
        }
        this.beforeBaseParams = params;
        this.requestUrl = "/commons/GridData.do";

        if (ld) {
            if (this.pageSize != 0) {
                if (ld == true) {
                    ld = { start: 0, limit: this.pageSize };
                } else {
                    ld['start'] = 0;
                    ld['limit'] = this.pageSize;
                }
            }
            if (typeof (ld) == 'object') {
                for (var p in ld) {
                    this.beforeBaseParams[p] = ld[p];
                }
            }

        }

        var st = new Ext.data.Store({
            autoLoad: ld,
            proxy: new Ext.data.HttpProxy({
                url: this.requestUrl,
                timeout: 9999999
            }),
            reader: new Ext.data.JsonReader()
        });

        this.store = st;
        if (this.beforeBaseParams) {
            st.on("beforeload", function (store, options) {
                //Ext.applyIf(st.baseParams, me.beforeBaseParams); ext4.x不再支持
                Ext.apply(st.getProxy().extraParams, me.beforeBaseParams);
            })
        }
        st.on('exception', function () {
            BeidaSoft.Msg.info('数据加载发生异常！');
        });
        st.on('load', function (store, records, option) {
            this.fireEvent('load', records);
        }, this);

        this.callParent(arguments);
    },
    onRender: function () {
        this.callParent(arguments);
        //添加提示
        if (this.tip) {
            this.addTip();
        }
    },
    onDestroy: function () {
        this.callParent(arguments);
        if (this.mask) {
            delete this.mask;
        }
    },
    addTip: function () {//私有方法，添加提示
        var me = this;
        var tip = Ext.create('BeidaSoft.tip.ToolTip', {
            target: me.el,
            delegate: me.itemSelector,
            trackMouse: true,
            listeners: {
                beforeshow: function updateTipBody(tip) {
                    var record = me.getRecord(tip.triggerElement);
                    var template = Ext.create('BeidaSoft.XTemplate', me.tip.tpl);
                    var result = template.apply(record.data);
                    tip.update(result);
                }
            }
        });
    },
    load: function (para) {//相比较直接在程序中直接调用store的load方法该方法可以在翻页时自动维护查询参数
        var paraObj = { params: {} };
        if (para) {
            //            for (var p in para) {
            //                paraObj.params[p] = para[p];
            //                //this.store.setBaseParam(p, para[p]); //添加到baseParam，可以防止翻页时参数丢失（注意start和limit是自动的，不需要添加）
            //                //extjs 4.x不再支持store的setBaseParam方法
            //            }

            Ext.apply(this.store.getProxy().extraParams, para);
        }
        if (this.pageSize > 0) {
            paraObj.params.start = 0;
            paraObj.params.limit = Number(this.pageSize);
        }
        this.store.load(paraObj);
    }
});
