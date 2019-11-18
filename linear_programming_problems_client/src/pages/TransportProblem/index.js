import React, {useState} from 'react';
import PropTypes from 'prop-types';
import Select from "../../components/Select";
import FormControl from "@material-ui/core/FormControl";
import InputLabel from "@material-ui/core/InputLabel";
import MenuItem from "@material-ui/core/MenuItem";
import InputTable from "../../components/InputTable";


const TransportProblem = (props) => {
    const select_options = [
        {value:2,label:"2"},
        {value:3,label:"3"},
        {value:4,label:"4"},
        {value:5,label:"5"},
    ];
    const [supplyCount, setSupplyCount] = useState(3);
    const [demandCount, setDemandCount] = useState(3);
    const handelSelectChange=(e)=>{
        if(e.target.name==='supplyCount') setSupplyCount(+e.target.value);
        else setDemandCount(+e.target.value);
    };
    return (
        <div>
            <Select
            name='supplyCount'
            value={supplyCount}
            onChange={handelSelectChange}
            label='Склади'
            options={select_options}
            />
            <Select
                name='demandCount'
                value={demandCount}
                onChange={handelSelectChange}
                label='Споживачі'
                options={select_options}
            />
            <InputTable supplyCount={supplyCount} demandCount={demandCount}/>
        </div>
    );
};

TransportProblem.propTypes = {};
TransportProblem.defaultProps = {};

export default TransportProblem;
